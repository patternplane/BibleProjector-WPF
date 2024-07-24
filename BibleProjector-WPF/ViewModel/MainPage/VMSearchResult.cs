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
        public string PreviewContent { get { return searchData.previewContent; } }
        public ShowContentType Type { get { return searchData.data.getDataType(); } }
        private module.SearchData searchData;

        public VMSearchResult(module.SearchData data)
        {
            this.searchData = data;
            this.DisplayTitle = data.displayName;
        }

        public module.Data.ShowData getData()
        {
            return searchData.data;
        }
    }
}
