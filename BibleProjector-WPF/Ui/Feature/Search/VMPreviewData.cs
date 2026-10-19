using BibleProjector_WPF.module.Data;
using BibleProjector_WPF.ViewModel.Option;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public bool isAvailable { get { return data.getData().isAvailData(); } }

        public BindingList<VMSongFrameFile> SongFrameFilesSource { get { return module.ProgramOption.SongFrameFiles; } }
        public VMSongFrameFile SongFrameFileDefault
        {
            get
            {
                if (Type != ShowContentType.Song)
                    return null;
                SongDataTypeEnum type = ((SongData)data.getData()).songType;
                if (type == SongDataTypeEnum.CCM)
                    return module.ProgramOption.DefaultCCMFrame;
                if (type == SongDataTypeEnum.HYMN)
                    return module.ProgramOption.DefaultHymnFrame;
                return null;
            }
        }

        public ICommand CReserveWithFrame { get; private set; }


        // handler
        private ICommand reserveThisWithFrame;

        public VMPreviewData(IPreviewData data, ICommand reserveThisWithFrame, bool isAdded = false, bool isModified = false)
        {
            this.reserveThisWithFrame = reserveThisWithFrame;
            setData(data, isAdded, isModified);
        }

        public void setData(IPreviewData data, bool isAdded, bool isModified)
        {
            this.data = data;
            this.isAdded = isAdded;
            this.isModified = isModified;
            CReserveWithFrame = new RelayCommand((obj) => ReserveWithFrame((VMSongFrameFile)obj));
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }

        public ShowData getData()
        {
            return data.getData();
        }

        private void ReserveWithFrame(VMSongFrameFile songFrame)
        {
            if (getData() is SongData)
                reserveThisWithFrame?.Execute(songFrame);
        }

        public virtual void update()
        {
            isModified = true;
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }
    }
}
