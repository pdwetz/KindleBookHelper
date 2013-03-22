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

namespace KindleBookHelper.Core
{
    public class Poem
    {
        public const string PoemTag = "div";
        public const string PoemClass = "poem";

        public string Title { get; set; }
        public List<Stanza> Stanzas { get; set; }

        public Poem(string sRaw)
        {
            Stanzas = new List<Stanza>();
            List<string> lines = sRaw.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            // While could do this, still need to account for its location; so, just handling it in full loop
            //Title = lines.First(x => !string.IsNullOrWhiteSpace(x));

            for (int i = 0; i < lines.Count; i++)
            {
                // Until we get a title, ignore all white space and related logic
                if (Title == null)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        Title = lines[i];
                    }
                    continue;
                }

                // Whitespace indicates an end to an existing stanza and a start of a new one
                // (note, we'll need to ignore empty ones at the end with this approach)
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    Stanzas.Add(new Stanza());
                    continue;
                }
                Stanzas.Last().Lines.Add(lines[i]);
            }
        }

        public void ConvertToHtml(ref StringBuilder sb)
        {
            if (string.IsNullOrWhiteSpace(Title) || Stanzas.Count == 0)
            {
                return;
            }
            sb.AppendFormat("<{0} class=\"{1}\">{2}", PoemTag, PoemClass, Environment.NewLine);
            sb.AppendFormat("<a id=\"{0}\"></a>{1}", Title.URLFriendly(), Environment.NewLine);
            sb.AppendFormat("<h1>{0}</h1>{1}", Title, Environment.NewLine);
            for (int i = 0; i < Stanzas.Count; i++)
            {
                Stanzas[i].ConvertToHtml(ref sb);
            }
            sb.AppendFormat("</{0}>{1}", PoemTag, Environment.NewLine);
        }
    }
}