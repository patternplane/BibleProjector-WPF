using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public abstract class SearchData : IComparable, Data.IPreviewData
    {
        public abstract string getdisplayName(bool isModified);
        public abstract string getPreviewContent();
        public int searchDistance { get; protected set; }
        protected Data.ShowData data { get; set; }

        public Data.ShowData getData()
        {
            return data;
        }

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
