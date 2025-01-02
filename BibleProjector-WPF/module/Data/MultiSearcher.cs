using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class MultiSearcher : ISearcher
    {
        ISearcher bibleSearcher = null;
        ISearcher songSearcher = null;
        ISearcher externPPTSearcher = null;

        public MultiSearcher(ISearcher bibleSearcher, ISearcher songSearcher, ISearcher externPPTSearcher)
        {
            this.bibleSearcher = bibleSearcher;
            this.songSearcher = songSearcher;
            this.externPPTSearcher = externPPTSearcher;
        }

        public ICollection<SearchData> getSearchResult(string phrase)
        {
            List<SearchData> searchResults = new List<SearchData>();
            searchResults.AddRange(externPPTSearcher.getSearchResult(phrase));
            searchResults.AddRange(bibleSearcher.getSearchResult(phrase));
            searchResults.AddRange(songSearcher.getSearchResult(phrase));

            //searchResults.Sort();
            return searchResults;
        }
    }
}
