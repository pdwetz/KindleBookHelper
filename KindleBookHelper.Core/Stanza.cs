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
    public class Stanza
    {
        public const string StanzaTag = "div";
        public const string StanzaClass = "stanza";
        public const string LineTag = "p";

        public List<string> Lines { get; set; }
        public Stanza() { Lines = new List<string>(); }

        public void ConvertToHtml(ref StringBuilder sb)
        {
            if (Lines.Count == 0)
            {
                return;
            }
            sb.AppendFormat("<{0} class=\"{1}\">{2}", StanzaTag, StanzaClass, Environment.NewLine);
            for (int i = 0; i < Lines.Count; i++)
            {
                sb.AppendFormat("<{0}>{1}</{0}>{2}", LineTag, Lines[i], Environment.NewLine);
            }
            sb.AppendFormat("</{0}>{1}", StanzaTag, Environment.NewLine);
        }
    }
}