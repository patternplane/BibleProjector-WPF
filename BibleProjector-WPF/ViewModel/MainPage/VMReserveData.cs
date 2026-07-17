using BibleProjector_WPF.ViewModel.Option;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMReserveData : ViewModel
    {
        // ============ External Dependency ============ 

        public static module.ShowStarter showStarter;



        public string DisplayTitle { get; set; } = null;
        int _MyIdx;
        public int MyIdx 
        {
            get
            {
                return _MyIdx;
            }
            set
            {
                _MyIdx = value;
                updateDisplayTitle();
            }
        }
        public ReserveViewType ViewType { get; set; }
        public module.Data.ShowData Data;
        public ShowContentType? ContentType { get { return Data?.getDataType(); } }

        public BindingList<VMSongFrameFile> SongFrameFilesSource { get { return module.ProgramOption.SongFrameFiles; } }

        public ICommand CItemShowWithFrame { get; private set; }

        string title;

        private VMReserveData(string title, module.Data.ShowData data, ReserveViewType type)
        {
            this.title = title;
            this.Data = data;
            this.ViewType = type;

            this.CItemShowWithFrame = new RelayCommand(request => showSongDataWithFrame((VMSongFrameFile)request));

            updateDisplayTitle();
        }

        public VMReserveData(string title, ReserveViewType type)
            : this(title, null, type)
        { 
        }

        public VMReserveData(module.Data.ShowData data, ReserveViewType type = ReserveViewType.NormalItem)
            : this(data.getTitle1() + " " + data.getTitle2(), data, type)
        {
        }

        // ================ Command ================

        private void showSongDataWithFrame(VMSongFrameFile songFrame)
        {
            if (Data is module.Data.SongData songData)
            {
                songData.pptFrameFullPath = songFrame.Path;
                showStarter.Show(songData);
            }
        }

        // ================ Methods ================

        public void updateDisplayTitle()
        {
            DisplayTitle = MyIdx + ". " + title;
            if (MyIdx < 10)
                DisplayTitle = "0" + DisplayTitle;
            OnPropertyChanged("DisplayTitle");
        }

        public void pasteDisplayTitle(string copiedTitle)
        {
            this.DisplayTitle = copiedTitle;
            OnPropertyChanged("DisplayTitle");
        }
    }
}
