using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMSearchResult : ViewModel
    {
        public string DisplayTitle { get { return searchData.getdisplayName(); } }
        public string PreviewContent { get { return searchData.getPreviewContent(); } }
        public ShowContentType Type { get { return searchData.data.getDataType(); } }
        private module.SearchData searchData;

        public void update()
        {
            searchData.update();
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }

        public VMSearchResult(module.SearchData data)
        {
            this.searchData = data;
        }

        public module.Data.ShowData getData()
        {
            return searchData.data;
        }
    }
}
