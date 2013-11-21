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
using System.IO;
using System.Reflection;

namespace KindleBookHelper.Core
{
	public class FileUtilities
	{
		private FileUtilities() {}

		public static string LoadTextFile(string sFilePath)
		{
			string sReturn = "";
            using (StreamReader sr = File.OpenText(sFilePath))
            {
                sReturn = sr.ReadToEnd();
                sr.DiscardBufferedData();
                sr.Close();
            }
            return sReturn;
		}

        public static string LoadTextResource(Assembly assembly, string sResourceName)
        {
            string sReturn = "";
            using (var sr = new StreamReader(assembly.GetManifestResourceStream(sResourceName)))
            {
                sReturn = sr.ReadToEnd();
                sr.DiscardBufferedData();
                sr.Close();
            }
            return sReturn;
        }

        public static void CreateDirectoryIfMissing(string sDirectoryPath)
        {
            if (!Directory.Exists(sDirectoryPath))
            {
                Directory.CreateDirectory(sDirectoryPath);
            }
        }

		public static void WriteTextFile(string sFilePath, string sData)
		{
            string sDir = Path.GetDirectoryName(sFilePath);
            CreateDirectoryIfMissing(sDir);
            using (StreamWriter sw = File.CreateText(sFilePath))
            {
                sw.Write(sData);
                sw.Flush();
                sw.Close();
            }
		}
	}
}