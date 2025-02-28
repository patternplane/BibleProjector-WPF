using BibleProjector_WPF.module.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMPreviewData : ViewModel, IVMPreviewData
    {
        private IPreviewData data;
        private bool isModified = false;
        private bool isAdded = false;

        public string DisplayTitle { get { return (isAdded ? "(추가됨) " : "") + (isModified ? "[수정됨] " : "") + data.getdisplayName(isModified); } }
        public string PreviewContent { get { return data.getPreviewContent(); } }
        public ShowContentType Type { get { return data.getData().getDataType(); } }

        public VMPreviewData(IPreviewData data, bool isAdded = false, bool isModified = false)
        {
            setData(data, isAdded, isModified);
        }

        public void setData(IPreviewData data, bool isAdded, bool isModified)
        {
            this.data = data;
            this.isAdded = isAdded;
            this.isModified = isModified;
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }

        public ShowData getData()
        {
            return data.getData();
        }

        public void update()
        {
            isModified = true;
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }
    }
}
