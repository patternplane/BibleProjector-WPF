using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class ExternPPTSearcher : ISearcher
    {
        ExternPPTManager manager;

        public ExternPPTSearcher(ExternPPTManager manager)
        {
            this.manager = manager;
        }

        public ICollection<SearchData> getSearchResult(string phrase)
        {
            List<Data.ExternPPTSearchData> result = new List<Data.ExternPPTSearchData>(5);

            foreach (Data.ExternPPTData item in manager.getDataList())
                if (StringKMP.HasPattern(item.fileName, phrase, delegate (char a, char b) { return a == b; }, false))
                    result.Add(new Data.ExternPPTSearchData(
                        item, 
                        0));

            return result.ToArray();

            // 유사 검색은 급한대로 우선 제쳐두고...
            /*
            const int SEARCH_DISTANCE_LIMIT = 1;

            LevenshteinDistance ld = new LevenshteinDistance();
            KorString ks = new KorString();

            Data.ExternPPTData[] originList = manager.getDataList();
            List<Data.ExternPPTSearchData> result = new List<Data.ExternPPTSearchData>(5);

            int searchDis;
            int phraseLen;
            for (int i = 0; i < originList.Length; i++)
            {
                searchDis = ld.getLevenDis_min(phrase, originList[i].fileName);
                phraseLen = ks.GetAddCost(phrase);

                if (searchDis <= ((phraseLen / 2.5) * SEARCH_DISTANCE_LIMIT))
                    result.Add(
                        new Data.ExternPPTSearchData(
                            originList[i]
                            , searchDis));
            }

            result.Sort((a, b) => (a.searchDistance > b.searchDistance ? 1 : (a.searchDistance == b.searchDistance ? 0 : -1)));
            return result.ToArray();
            */
        }
    }
}
