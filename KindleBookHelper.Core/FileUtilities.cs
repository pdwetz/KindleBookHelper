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
using System.IO;
using System.Reflection;

namespace KindleBookHelper.Core
{
	public static class FileUtilities
	{
		public static string LoadTextFile(string filePath)
		{
			var data = "";
            using (StreamReader sr = File.OpenText(filePath))
            {
                data = sr.ReadToEnd();
                sr.DiscardBufferedData();
                sr.Close();
            }
            return data;
		}

        public static string LoadTextResource(Assembly assembly, string resourceName)
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

        public static void CreateDirectoryIfMissing(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

		public static void WriteTextFile(string filePath, string data)
		{
            var dirName = Path.GetDirectoryName(filePath);
            CreateDirectoryIfMissing(dirName);
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }
		}
	}
}