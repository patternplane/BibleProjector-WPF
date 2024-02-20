using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class Searcher : ISearcher
    {
        ISearcher bibleSearcher = null;
        ISearcher songSearcher = null;

        public Searcher(ISearcher bibleSearcher, ISearcher songSearcher)
        {
            this.bibleSearcher = bibleSearcher;
            this.songSearcher = songSearcher;
        }

        public ICollection<ISearchData> getSearchResult(string phrase)
        {
            List<ISearchData> searchResults = new List<ISearchData>();
            searchResults.AddRange(bibleSearcher.getSearchResult(phrase));
            searchResults.AddRange(songSearcher.getSearchResult(phrase));

            searchResults.Sort();
            return searchResults;
        }
    }
}
