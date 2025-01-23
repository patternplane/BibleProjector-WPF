using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public abstract class SearchData : IComparable
    {
        protected bool isModified = false;
        public abstract string getdisplayName();
        public abstract string getPreviewContent();
        public int searchDistance { get; protected set; }
        public Data.ShowData data { get; protected set; }

        public void update()
        {
            isModified = true;
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
