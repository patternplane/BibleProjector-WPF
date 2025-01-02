using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    class SongSearchData : SearchData
    {
        public SongSearchData(SongData data, string foundPhrase ,int searchDistance)
        {
            this.data = data;
            this.displayName = foundPhrase;
            this.searchDistance = searchDistance;

            StringBuilder preview = new StringBuilder();
            string[] lyrics = data.songContent.getContents();
            for (int i = 0; i < lyrics.Length; i++) 
            {
                preview.Append(" [ ").Append(i + 1).Append("절 ]").Append('\n');
                preview.Append(lyrics[i]);
                if (i < lyrics.Length - 1)
                    preview.Append("\n\n");
            }
            this.previewContent = preview.ToString();
        }
    }
}
