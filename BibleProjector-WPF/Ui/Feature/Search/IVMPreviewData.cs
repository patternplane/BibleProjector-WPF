using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public interface IVMPreviewData
    {
        string DisplayTitle { get; }
        string PreviewContent { get; }
        ShowContentType Type { get; }
        module.Data.ShowData getData();
        void update();
    }
}
