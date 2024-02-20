using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class SongData 
    {
        public string songTitle { get; set; }
        public SongContent songContent { get; set; }

        public SongData(string songTitle, SongContent songContent)
        {
            this.songTitle = songTitle;
            this.songContent = songContent;
        }
    }
}
