/*
    KindleBookHelper - Converts raw text file to html format that can be consumed by KindleGen.
    Copyright (C) 2016 Peter Wetzel

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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Nustache.Core;

namespace KindleBookHelper.Core
{
    public class Book
    {
        public const string EndPlaceholder = "[END]";

        public string RawFilePath { get; set; }
        public string Title { get; set; }
        public Guid Id { get; set; }
        public string CoverFileName { get; set; }
        public string Publisher { get; set; }
        public string Creator { get; set; }
        public string Author { get; set; }
        public string AuthorAlphabetical { get; set; }
        public string AuthorDescription { get; set; }
        public string AuthorWebsite { get; set; }
        public string Forward { get; set; }

        [JsonIgnore]
        public string TitleFileSafe { get { return Title.URLFriendly(); } }

        [JsonIgnore]
        public int EndNavPlayOrder { get { return 3 + Poems.Count; } }
        // TODO Pre-work free-form content
        [JsonIgnore]
        public List<Poem> Poems { get; set; }
        // TODO Post-work free-form content
        [JsonIgnore]
        public string OriginalText { get; set; }

        [JsonIgnore]
        public int Copyright { get { return DateTime.Now.Year; } }

        private readonly Assembly _assembly;

        public Book()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        public static Book Initialize(string sJsonFilePath)
        {
            var book = JsonConvert.DeserializeObject<Book>(File.ReadAllText(sJsonFilePath));
            if (book.Id == Guid.Empty)
            {
                book.Id = Guid.NewGuid();
                File.WriteAllText(sJsonFilePath, JsonConvert.SerializeObject(book, Formatting.Indented));
            }
            return book;
        }
        
        public void Process()
        {
            if (string.IsNullOrWhiteSpace(RawFilePath))
            {
                throw new ArgumentException("RawFilePath parameter must be set");
            }

            OriginalText = FileUtilities.LoadTextFile(RawFilePath);

            Parse();

            if (Poems.Count == 0)
            {
                throw new ApplicationException("No poems found in source");
            }

            var sTargetDirectoryPath = Path.GetDirectoryName(RawFilePath);
            RenderTemplate(sTargetDirectoryPath, "html");
            RenderTemplate(sTargetDirectoryPath, "ncx");
            RenderTemplate(sTargetDirectoryPath, "opf");

            var sCssFilePath = Path.Combine(sTargetDirectoryPath, "poetry.css");
            if (!File.Exists(sCssFilePath))
            {
                var sCss = FileUtilities.LoadTextResource(_assembly, "KindleBookHelper.Core.templates.poetry.css");
                FileUtilities.WriteTextFile(sCssFilePath, sCss);
            }
        }

        private void RenderTemplate(string sTargetDirectoryPath, string sTemplate)
        {
            var targetFilePath = Path.Combine(sTargetDirectoryPath, $"{TitleFileSafe}.{sTemplate}");
            using (var reader = new StreamReader(_assembly.GetManifestResourceStream($"KindleBookHelper.Core.templates.{sTemplate}.template")))
            {
                using (var writer = File.CreateText(targetFilePath))
                {
                    Render.Template(reader, this, writer);
                }
            }
        }

        public void Parse()
        {
            Poems = new List<Poem>();
            int iPos = 0;
            while (iPos >= 0 && iPos < OriginalText.Length)
            {
                int iEndPos = OriginalText.IndexOf(EndPlaceholder, iPos);
                if (iEndPos < 0)
                {
                    break;
                }
                string sRawPoem = OriginalText.Substring(iPos, iEndPos - iPos);
                Poems.Add(new Poem(sRawPoem));
                iPos = iEndPos + EndPlaceholder.Length;
            }
        }
    }
}