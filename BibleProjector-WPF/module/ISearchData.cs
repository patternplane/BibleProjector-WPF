using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public interface ISearchData : IComparable
    {
        string displayName { get; }
        string previewContent { get; }
        int searchDistance { get; }
        Data.ShowData getData();
    }
}
