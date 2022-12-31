using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class Song : IReserveOptionViewModel, INotifyPropertyChanged
    {

        // =============================== 비 바인딩 속성 ================================

        ReserveCollectionUnit selectionValue = null;

        const int CCM_SONG_CODE = 0;
        const int HYMN_SONG_CODE = 1;
        BindingList<SingleLyric>[] songLists = { LyricViewModel.LyricList, LyricViewModel.HymnList };

        // =============================== 바인딩 속성 ================================

        int _LinePerPageText = 2;
        public string LinePerPageText { get { return _LinePerPageText.ToString(); } set { 
                string num = module.StringModifier.makeOnlyNum(value);
                if (num.Length == 0)
                    _LinePerPageText = 0;
                else
                    _LinePerPageText = int.Parse(num);
            } }
        
        public BindingList<module.SongFrameFile> SongFrameList { get; set; }
            = module.ProgramOption.SongFrameFiles;
        module.SongFrameFile _SongFrameSelection;
        public module.SongFrameFile SongFrameSelection { get { return _SongFrameSelection; } set { _SongFrameSelection = value; onPropertyChanged(nameof(SongFrameSelection)); } }

        bool _isHymn;
        public bool isHymn { get { return _isHymn; } set { _isHymn = value; onPropertyChanged(nameof(isHymn)); onPropertyChanged(nameof(SongList)); } }
        public BindingList<SingleLyric> SongList { get { return ((isHymn) ? songLists[HYMN_SONG_CODE] : songLists[CCM_SONG_CODE]); } }
        SingleLyric _SongSelection;
        public SingleLyric SongSelection { get { return _SongSelection; } set { _SongSelection = value; if (value != null) ChangeLyricSelection(value); } }

        // =============================== 메소드 ================================

        void ChangeLyricSelection(SingleLyric lyric)
        {
            selectionValue.ChangeReserveData(new module.SongReserveDataUnit(lyric));
        }

        public void ShowContent()
        {
            SingleLyric songSelection = ((module.SongReserveDataUnit)selectionValue.reserveData).lyric;

            if (SongFrameSelection == null)
                System.Windows.MessageBox.Show("찬양 출력 틀ppt를 등록해주세요!", "ppt 틀 선택되지 않음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else if (songSelection == null)
                System.Windows.MessageBox.Show("출력할 찬양곡을 선택해주세요!", "찬양곡 선택되지 않음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
            {
                new module.ShowStarter().SongShowStart(songSelection, int.Parse(LinePerPageText), SongFrameSelection.Path);
            }
        }

        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            selectionValue = data[0];

            if (((module.SongReserveDataUnit)(selectionValue.reserveData)).isHymn)
            {
                if (module.ProgramOption.DefaultHymnFrame != null)
                    SongFrameSelection = module.ProgramOption.DefaultHymnFrame;
            }
            else if (module.ProgramOption.DefaultCCMFrame != null)
                SongFrameSelection = module.ProgramOption.DefaultCCMFrame;

            module.SongReserveDataUnit reserveData = (module.SongReserveDataUnit)selectionValue.reserveData; 
            if (reserveData.isHymn)
                isHymn = true;
            else
                isHymn = false;
            _SongSelection = reserveData.lyric;
            onPropertyChanged(nameof(SongSelection));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
