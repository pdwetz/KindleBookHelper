/*
    KindleBookHelper - Converts raw text file to html format that can be consumed by KindleGen.
    Copyright (C) 2018 Peter Wetzel

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Nustache.Core;
using Serilog;
using WetzUtilities;

namespace KindleBookHelper.Core
{
    public class BookProcessor
    {
        private readonly string _jsonFilePath;
        private readonly string _endPlaceholder;
        private readonly Assembly _assembly;

        public BookProcessor(string jsonFilePath, string endPlaceholder = Book.DefaultEndPlaceholder)
        {
            if (string.IsNullOrWhiteSpace(jsonFilePath))
            {
                throw new ArgumentException($"{nameof(jsonFilePath)} parameter must be set");
            }
            _jsonFilePath = jsonFilePath;
            _endPlaceholder = endPlaceholder;
            _assembly = Assembly.GetExecutingAssembly();
        }

        public Book Process()
        {
            Log.Information("Processing book file {jsonFilePath}", _jsonFilePath);
            var book = JsonConvert.DeserializeObject<Book>(File.ReadAllText(_jsonFilePath));
            if (book.Id == Guid.Empty)
            {
                book.Id = Guid.NewGuid();
                Log.Information("Saving new unique Id");
                File.WriteAllText(_jsonFilePath, JsonConvert.SerializeObject(book, Formatting.Indented));
            }
            if (string.IsNullOrWhiteSpace(book.RawFilePath))
            {
                Log.Error("RawFilePath is required");
                return null;
            }
            book.EndPlaceholder = _endPlaceholder;
            book.OriginalText = FileUtilities.LoadTextFile(book.RawFilePath);
            book.Parse();
            if (!book.Poems.Any())
            {
                Log.Error("No poems found in source");
                return null;
            }
            Log.Information("Found {Count} poems", book.Poems.Count);
            var targetDirectoryPath = Path.GetDirectoryName(book.RawFilePath);
            RenderTemplate(targetDirectoryPath, "html", book);
            RenderTemplate(targetDirectoryPath, "ncx", book);
            RenderTemplate(targetDirectoryPath, "opf", book);
            var cssFilePath = Path.Combine(targetDirectoryPath, "poetry.css");
            if (!File.Exists(cssFilePath))
            {
                var css = LoadTextResource(_assembly, "KindleBookHelper.Core.templates.poetry.css");
                FileUtilities.WriteTextFile(targetDirectoryPath, "poetry.css", css);
            }
            Log.Information("Finished processing book {Title}", book.Title);
            return book;
        }

        private string LoadTextResource(Assembly assembly, string resourceName)
        {
            var data = "";
            using (var sr = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                data = sr.ReadToEnd();
                sr.DiscardBufferedData();
                sr.Close();
            }
            return data;
        }

        private void RenderTemplate(string targetDirectoryPath, string template, Book book)
        {
            var targetFilePath = Path.Combine(targetDirectoryPath, $"{book.TitleFileSafe}.{template}");
            Log.Debug("Rendering template {template}", template);
            using (var reader = new StreamReader(_assembly.GetManifestResourceStream($"KindleBookHelper.Core.templates.{template}.template")))
            {
                using (var writer = File.CreateText(targetFilePath))
                {
                    Render.Template(reader, book, writer);
                }
            }
        }
    }
}