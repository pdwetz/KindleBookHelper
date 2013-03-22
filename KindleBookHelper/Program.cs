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
using System.Configuration;
using KindleBookHelper.Core;

namespace KindleBookHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            KindleBookHelperSettings settings = new KindleBookHelperSettings();
            settings.SourceFilePath = ConfigurationManager.AppSettings["KindleBookHelper.SourceFilePath"];
            settings.TargetFolderPath = ConfigurationManager.AppSettings["KindleBookHelper.TargetFolderPath"];
            settings.TargetName = ConfigurationManager.AppSettings["KindleBookHelper.TargetName"];

            Book book = new Book(settings);
            book.Process();

            Console.WriteLine("Done");
        }
    }
}