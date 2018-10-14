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
using CommandLine;
using KindleBookHelper.Core;

namespace KindleBookHelper
{
    public class ProgramOptions
    {
        [Option('f', "filepath", Required = true, HelpText = "Full path of file to process.")]
        public string FilePath { get; set; }

        [Option('e', "end", Default = Book.DefaultEndPlaceholder, Required = false, HelpText = "Text that indicates the end of a poem.")]
        public string EndPlaceholder { get; set; }

        [Option('p', "pause", Required = false, HelpText = "Pause on program completion.")]
        public bool PauseAtCompletion { get; set; }
    }
}