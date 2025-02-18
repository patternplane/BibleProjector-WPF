using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class BibleSearchData : SearchData
    {
        private bool isShort;

        public BibleSearchData(BibleData data, bool isShort, int searchDistance)
        {
            this.data = data;
            this.isShort = isShort;
            this.searchDistance = searchDistance;
        }

        public override string getdisplayName()
        {
            string displayName;
            BibleData typedData = (BibleData)data;

            if (isShort)
                displayName = $"({typedData.getBibleTitle_Abr()}) {typedData.getBibleTitle()}";
            else
                displayName = typedData.getBibleTitle();

            if (typedData.chapter != -1)
                displayName += $" {typedData.chapter}장 ";
            if (typedData.verse != -1)
                displayName += $"{typedData.verse}절";

            if (isModified)
                return "[수정됨] " + displayName;
            else
                return displayName;
        }

        public override string getPreviewContent()
        {
            return ((BibleData)data).getBibleContent();
        }
    }
}
