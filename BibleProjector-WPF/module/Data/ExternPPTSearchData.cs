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
            this.displayName = " (외부자료) " + data.fileName;
            this.previewContent = "";
            this.searchDistance = searchDistance;
        }
    }
}
