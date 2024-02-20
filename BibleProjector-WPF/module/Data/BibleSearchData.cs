using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class BibleSearchData : ISearchData
    {

        BibleData data;

        public int searchDistance { get; }
        public int CompareTo(object obj)
        {
            return
                (searchDistance > ((ISearchData)obj).searchDistance) ?
                1 :
                (searchDistance == ((ISearchData)obj).searchDistance ?
                0 :
                -1);
        }

        public string displayName { get; }

        public string previewContent { get; }

        public BibleSearchData(BibleData data, bool isShort, int searchDistance)
        {
            this.data = data;
            this.searchDistance = searchDistance;

            if (isShort)
                displayName = $"({data.getBibleTitle_Abr()}) {data.getBibleTitle()}";
            else
                displayName = data.getBibleTitle();

            if (data.chapter != -1)
                displayName += $" {data.chapter}절";
            if (data.verse != -1)
                displayName += $"{data.verse}절";

            previewContent = data.getBibleContent();
        }
    }
}
