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
using KindleBookHelper.Core;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using WetzUtilities;

namespace KindleHelper.Test
{
    [TestFixture]
    public class BookTester
    {
        private string _sourceFilePath;

        [SetUp]
        public void Setup()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _sourceFilePath = Path.Combine(path, "test.json");
            Console.WriteLine(_sourceFilePath);
        }

        [Test]
        public void process()
        {
            var directoryPath = Path.GetDirectoryName(_sourceFilePath);
            var rawFilePath = Path.Combine(directoryPath, "raw.txt");
            JObject settings = JObject.FromObject(new
            {
                RawFilePath = rawFilePath,
                Title = "Test",
                Id = Guid.NewGuid(),
                CoverFileName = "cover.jpg",
                Publisher = "Test Publisher",
                Creator = "Test Creator",
                Author = "Test Author",
                AuthorAlphabetical = "Author, Test",
            });
            FileUtilities.WriteTextFile(directoryPath, Path.GetFileName(_sourceFilePath), settings.ToString());
            string endPlaceholder = "--end--";
            string sample = $@"
A Title

First Line
Second Line

Another Stanze
Last Line

{endPlaceholder}

Another Title

Another line
Yet Another line

{endPlaceholder}
";
            FileUtilities.WriteTextFile(directoryPath, "raw.txt", sample);

            var p = new BookProcessor(_sourceFilePath, endPlaceholder);
            var book = p.Process();

            var targetFilePath = Path.Combine(directoryPath, $"{book.TitleFileSafe}.html");
            Console.WriteLine(targetFilePath);
            Assert.IsTrue(File.Exists(targetFilePath));

            targetFilePath = Path.Combine(directoryPath, $"{book.TitleFileSafe}.ncx");
            Console.WriteLine(targetFilePath);
            Assert.IsTrue(File.Exists(targetFilePath));

            targetFilePath = Path.Combine(directoryPath, $"{book.TitleFileSafe}.opf");
            Console.WriteLine(targetFilePath);
            Assert.IsTrue(File.Exists(targetFilePath));
        }
    }
}