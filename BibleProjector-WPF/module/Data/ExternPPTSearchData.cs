using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    class ExternPPTSearchData : SearchData
    {
        public ExternPPTSearchData(ExternPPTData data, int searchDistance)
        {
            this.data = data;
            this.searchDistance = searchDistance;
        }

        public override string getdisplayName(bool isModified)
        {
            return " (외부자료) " + ((ExternPPTData)data).fileName;
        }

        public override string getPreviewContent()
        {
            return "";
        }
    }
}
