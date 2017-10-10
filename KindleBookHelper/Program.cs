/*
    KindleBookHelper - Converts raw text file to html format that can be consumed by KindleGen.
    Copyright (C) 2017 Peter Wetzel

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
            Console.WriteLine("KindleBookHelper");
            Console.WriteLine("Copyright (C) 2017 Peter Wetzel");
            Console.WriteLine("This program comes with ABSOLUTELY NO WARRANTY; for details see license.txt.");
            string sFilePath = args == null || args.Length == 0 ? ConfigurationManager.AppSettings["KindleBookHelper.SourceFilePath"] : args[0];
            Console.WriteLine($"Processing '{sFilePath}'...");
            Book book = Book.Initialize(sFilePath);
            book.Process();
            Console.WriteLine("Done. Press any key to continue.");
            Console.ReadLine();
        }
    }
}