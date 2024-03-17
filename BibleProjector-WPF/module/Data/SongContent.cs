using System;
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
            foreach (string lyric in lyrics)
                result.Append(lyric).Append(" ");
            return result.ToString();
        }

        public string getContentByVerse(int page)
        {
            if (page < 0 || page > lyricCount)
                return null;

            return lyrics[page];
        }
    }
}
