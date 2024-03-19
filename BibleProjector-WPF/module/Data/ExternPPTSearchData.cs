using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    class ExternPPTSearchData : ISearchData
    {
        public string displayName => throw new NotImplementedException();

        public string previewContent => throw new NotImplementedException();

        public int searchDistance => throw new NotImplementedException();
        public int CompareTo(object obj)
        {
            return
                (searchDistance > ((ISearchData)obj).searchDistance) ?
                1 :
                (searchDistance == ((ISearchData)obj).searchDistance ?
                0 :
                -1);
        }

        public ShowData getData()
        {
            throw new NotImplementedException();
        }
    }
}
