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
        // ============ External Dependency ============ 

        public static module.ShowStarter showStarter;



        private IPreviewData data;
        private bool isModified = false;
        private bool isAdded = false;

        public string DisplayTitle { get { return (isAdded ? "(추가됨) " : "") + (isModified ? "[수정됨] " : "") + data.getdisplayName(isModified); } }
        public string PreviewContent { get { return data.getPreviewContent(); } }
        public ShowContentType Type { get { return data.getData().getDataType(); } }

        public BindingList<VMSongFrameFile> SongFrameFilesSource { get { return module.ProgramOption.SongFrameFiles; } }

        public ICommand CItemShowWithFrame { get; private set; }


        public VMPreviewData(IPreviewData data, bool isAdded = false, bool isModified = false)
        {
            setData(data, isAdded, isModified);
        }

        public void setData(IPreviewData data, bool isAdded, bool isModified)
        {
            this.data = data;
            this.isAdded = isAdded;
            this.isModified = isModified;
            this.CItemShowWithFrame = new RelayCommand(request => showSongDataWithFrame((VMSongFrameFile)request));
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }

        public ShowData getData()
        {
            return data.getData();
        }

        private void showSongDataWithFrame(VMSongFrameFile songFrame)
        {
            if (getData() is SongData songData)
            {
                songData.pptFrameFullPath = songFrame.Path;
                showStarter.Show(songData);
            }
        }

        public virtual void update()
        {
            isModified = true;
            OnPropertyChanged(nameof(DisplayTitle));
            OnPropertyChanged(nameof(PreviewContent));
        }
    }
}
