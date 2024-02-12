using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class Song : ViewModel, IReserveOptionViewModel
    {

        // =============================== 비 바인딩 속성 ================================

        ReserveCollectionUnit selectionValue = null;

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
        public module.SongFrameFile SongFrameSelection { get { return _SongFrameSelection; } set { _SongFrameSelection = value; OnPropertyChanged(nameof(SongFrameSelection)); } }

        public BindingList<SingleLyric> CCMList { get; set; }
            = LyricViewModel.LyricList;
        SingleLyric _CCMSelection;
        public SingleLyric CCMSelection { get { return _CCMSelection; } set { _CCMSelection = value; ChangeLyricSelection(value); } }
        public BindingList<SingleHymn> HymnList { get; set; } 
            = LyricViewModel.HymnList;
        SingleHymn _HymnSelection;
        public SingleHymn HymnSelection { get { return _HymnSelection; } set { _HymnSelection = value; ChangeLyricSelection(value); } }

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
        }
    }
}
