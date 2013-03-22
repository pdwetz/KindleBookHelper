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
using System.Text;

namespace KindleBookHelper.Core
{
    public class Book
    {
        public const string EndPlaceholder = "[END]";
        public const int NcxNavPlayOrderStart = 3;

        public KindleBookHelperSettings Settings { get; set; }
        public List<Poem> Poems { get; set; }
        public string OriginalText { get; set; }

        public Book(KindleBookHelperSettings settings)
        {
            Settings = settings;
        }
        
        public void Process()
        {
            OriginalText = FileUtilities.LoadTextFile(Settings.SourceFilePath);

            Parse();

            string sHtml = ConvertToHtml();
            FileUtilities.WriteTextFile(Settings.TargetHtmlFilePath, sHtml);

            string sNcx = GetNcx();
            FileUtilities.WriteTextFile(Settings.TargetNcxFilePath, sNcx);
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

        public string ConvertToHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"toc\">");
            sb.AppendLine("<a id=\"toc\"></a>");
            sb.AppendLine("<h1>Table of Contents</h1>");
            sb.AppendLine("<ul>");
            foreach (Poem p in Poems)
            {
                sb.AppendFormat("<li><a href=\"#{0}\">{1}</a></li>{2}", p.Title.URLFriendly(), p.Title, Environment.NewLine);
            }
            sb.AppendLine("</ul>");
            sb.AppendLine("</div>");
            foreach (Poem p in Poems)
            {
                p.ConvertToHtml(ref sb);
            }
            return sb.ToString();
        }

        public string GetNcx()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Poems.Count; i++)
            {
                sb.AppendFormat("<navPoint id=\"{0}\" playOrder=\"{1}\">{2}", Poems[i].Title.URLFriendly(),
                    i + NcxNavPlayOrderStart, Environment.NewLine);
                sb.AppendFormat("<navLabel><text>{0}</text></navLabel>{1}", Poems[i].Title, Environment.NewLine);
                sb.AppendFormat("<content src=\"{0}#{1}\"/>{2}", Settings.TargetHtmlFileName, Poems[i].Title.URLFriendly(), Environment.NewLine);
                sb.AppendLine("</navPoint>");
            }
            return sb.ToString();
        }
    }
}