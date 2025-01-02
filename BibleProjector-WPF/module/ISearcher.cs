using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public interface ISearcher
    {
        ICollection<SearchData> getSearchResult(string phrase);
    }
}
