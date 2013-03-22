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
using System.IO;

namespace KindleBookHelper.Core
{
    public class KindleBookHelperSettings
    {
        public string SourceFilePath { get; set; }
        public string TargetFolderPath { get; set; }
        public string TargetName { get; set; }

        public string TargetHtmlFileName { get { return string.Format("{0}.html", TargetName); } }
        public string TargetNcxFileName { get { return string.Format("{0}.Ncx", TargetName); } }

        public string TargetHtmlFilePath { get { return Path.Combine(TargetFolderPath, TargetHtmlFileName); } }
        public string TargetNcxFilePath { get { return Path.Combine(TargetFolderPath, TargetNcxFileName); } }

        public KindleBookHelperSettings()
        {
            TargetName = "Generated";
        }
    }
}