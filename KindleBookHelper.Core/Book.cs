/*
    KindleBookHelper - Converts raw text file to html format that can be consumed by KindleGen.
    Copyright (C) 2019 Peter Wetzel

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
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using WetzUtilities;

namespace KindleBookHelper.Core
{
    public class Book
    {
        public const string DefaultEndPlaceholder = "[END]";

        public string RawFilePath { get; set; }
        public string Title { get; set; }
        public Guid Id { get; set; }
        public string CoverFileName { get; set; }
        public string Publisher { get; set; }
        public string Creator { get; set; }
        public string Author { get; set; }
        public string AuthorAlphabetical { get; set; }
        public string AuthorDescription { get; set; }
        public string AuthorWebsite { get; set; }
        public string Forward { get; set; }

        [JsonIgnore]
        public string TitleFileSafe => Title.URLFriendly();

        [JsonIgnore]
        public int EndNavPlayOrder => 3 + Poems.Count;
        // TODO Pre-work free-form content
        [JsonIgnore]
        public List<Poem> Poems { get; set; }
        // TODO Post-work free-form content
        [JsonIgnore]
        public string OriginalText { get; set; }

        [JsonIgnore]
        public int Copyright => DateTime.Now.Year;

        [JsonIgnore]
        public string EndPlaceholder { get; set; }

        public void Parse()
        {
            Poems = new List<Poem>();
            if (string.IsNullOrWhiteSpace(OriginalText))
            {
                Log.Error("OriginalText is required");
                return;
            }
            if (string.IsNullOrWhiteSpace(EndPlaceholder))
            {
                Log.Error("EndPlaceholder is required");
                return;
            }
            int i = 0;
            while (i >= 0 && i < OriginalText.Length)
            {
                int end = OriginalText.IndexOf(EndPlaceholder, i);
                if (end < 0)
                {
                    break;
                }
                var poem = OriginalText.Substring(i, end - i);
                Poems.Add(new Poem(poem));
                i = end + EndPlaceholder.Length;
            }
        }
    }
}