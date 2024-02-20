using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    class SongSearchData : ISearchData
    {
        public string displayName { get; } = null;
        public string previewContent { get; } = null;
        public int searchDistance { get; } = -1;
        public int CompareTo(object obj)
        {
            return
                (searchDistance > ((ISearchData)obj).searchDistance) ?
                1 :
                (searchDistance == ((ISearchData)obj).searchDistance ?
                0 :
                -1);
        }

        SongData data;

        public SongSearchData(SongData data, string foundPhrase ,int searchDistance)
        {
            this.data = data;
            this.displayName = foundPhrase;
            this.searchDistance = searchDistance;

            this.previewContent = data.songContent.getRawContent();
        }
    }
}
