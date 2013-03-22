/*
    KindleBookHelper - Converts raw text file to html format that can be consumed by KindleGen.
    Copyright (C) 2013 Peter Wetzel

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using KindleBookHelper.Core;
using System.IO;
using System.Reflection;

namespace KindleHelper.Test
{
    [TestFixture]
    public class BookTester
    {
        private KindleBookHelperSettings _settings;

        [SetUp]
        public void Setup()
        {
            _settings = new KindleBookHelperSettings();
            string sPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _settings.SourceFilePath = Path.Combine(sPath, "test.txt");
            _settings.TargetFolderPath = sPath;
        }

        [Test]
        public void process()
        {
            string sRaw = @"
A Title

First Line
Second Line

Another Stanze
Last Line

[END]

Another Title

Another line
Yet Another line

[END]
";
            FileUtilities.WriteTextFile(_settings.SourceFilePath, sRaw);
            Book book = new Book(_settings);
            book.Process();
            Assert.IsTrue(File.Exists(_settings.TargetHtmlFilePath));
            Assert.IsTrue(File.Exists(_settings.TargetNcxFilePath));
        }
    }
}
