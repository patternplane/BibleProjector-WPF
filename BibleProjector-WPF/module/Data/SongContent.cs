﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class SongContent
    {
        string[] lyrics;
        public int lyricCount = 0;

        public SongContent(string lyric)
        {
            lyrics = new string[1] { lyric };
            lyricCount = 1;
        }

        public SongContent(string[] lyrics)
        {
            this.lyrics = lyrics;
            lyricCount = lyrics.Length;
        }

        public string getRawContent()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < lyrics.Length; i++)
            {
                result.Append(lyrics[i]);
                if (i < lyrics.Length - 1)
                    result.Append('\n');
            }
            return result.ToString();
        }

        public string[] getContents()
        {
            return lyrics.ToArray();
        }

        public string getContentByVerse(int pageIdx)
        {
            if (pageIdx < 0 || pageIdx >= lyricCount)
                return null;

            return lyrics[pageIdx];
        }

        public void setContent(string content, int Idx)
        {
            if (Idx < 0 || Idx >= lyricCount
                || content == null)
                return;

            lyrics[Idx] = content;
        }
    }
}
