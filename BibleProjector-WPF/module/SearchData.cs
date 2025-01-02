using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public abstract class SearchData : IComparable
    {
        public string displayName { get; protected set; }
        public string previewContent { get; protected set; }
        public int searchDistance { get; protected set; }
        public Data.ShowData data { get; protected set; }

        public int CompareTo(object obj)
        {
            return
                (searchDistance > ((SearchData)obj).searchDistance) ?
                1 :
                (searchDistance == ((SearchData)obj).searchDistance ?
                0 :
                -1);
        }
    }
}
