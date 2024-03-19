using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMSearchResult : ViewModel
    {
        public string DisplayTitle { get; }
        module.ISearchData searchData;

        public VMSearchResult(module.ISearchData data)
        {
            this.searchData = data;
            this.DisplayTitle = data.displayName;
        }

        public module.Data.ShowData getData()
        {
            return searchData.getData();
        }
    }
}
