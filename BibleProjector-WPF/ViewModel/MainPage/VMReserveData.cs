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

        public VMSongFrameFile SelectedSongFramePath { get; set; }

        public BindingList<VMSongFrameFile> SongFrameFilesSource { get { return module.ProgramOption.SongFrameFiles; } }
        public VMSongFrameFile SongFrameFileDefault
        { 
            get {
                if (ContentType != ShowContentType.Song)
                    return null;
                module.Data.SongDataTypeEnum type = ((module.Data.SongData)Data).songType;
                if (type == module.Data.SongDataTypeEnum.CCM)
                    return module.ProgramOption.DefaultCCMFrame;
                if (type == module.Data.SongDataTypeEnum.HYMN)
                    return module.ProgramOption.DefaultHymnFrame;
                return null;
            } 
        }

        string title;

        private VMReserveData(string title, module.Data.ShowData data, ReserveViewType type)
        {
            this.title = title;
            this.Data = data;
            this.ViewType = type;

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
