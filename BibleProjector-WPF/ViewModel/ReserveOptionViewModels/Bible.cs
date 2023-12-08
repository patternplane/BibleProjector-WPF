using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class Bible : NotifyPropertyChanged, IReserveOptionViewModel
    {
        ReserveCollectionUnit selection;
        module.BibleReserveDataUnit bibleData { get { return (module.BibleReserveDataUnit)selection.reserveData; } }

        // ==================== 속성 ==================== 

        public string[] KuenList { get; set; } = Database.getBibleTitles_string();
        int[] _JangList;
        public int[] JangList { get { return _JangList; } set { _JangList = value; } }
        int[] _JeulList;
        public int[] JeulList { get { return _JeulList; } set { _JeulList = value; } }

        int _KuenSelection_index;
        public int KuenSelection_index { get { return _KuenSelection_index; } set { _KuenSelection_index = value; KuenChanged(); } }
        int _JangSelection;
        public int JangSelection { get { return _JangSelection; } set { _JangSelection = value; JangChanged(); } }
        int _JeulSelection;
        public int JeulSelection { get { return _JeulSelection; } set { _JeulSelection = value; JeulChanged(); } }

        void KuenChanged()
        {
            JangList = null;
            OnPropertyChanged(nameof(JangList));
            JeulList = null;
            OnPropertyChanged(nameof(JeulList));

            int JangCount = Database.getChapterCount((KuenSelection_index + 1).ToString("00")); ;
            JangList = new int[JangCount];

            for (int i = 1; i <= JangCount; i++)
                JangList[i-1] = i;
            OnPropertyChanged(nameof(JangList));
        }

        void JangChanged()
        {
            JeulList = null;
            OnPropertyChanged(nameof(JeulList));

            int JeulCount = Database.getVerseCount((KuenSelection_index + 1).ToString("00") + JangSelection.ToString("000"));
            JeulList = new int[JeulCount];

            for (int i = 1; i <= JeulCount; i++)
                JeulList[i - 1] = i;
            OnPropertyChanged(nameof(JeulList));
        }

        void JeulChanged()
        {
            selection.ChangeReserveData(new module.BibleReserveDataUnit(
                (KuenSelection_index + 1).ToString("00")
                , JangSelection.ToString("000")
                , JeulSelection.ToString("000")));
        }

        // ==================== 메소드 ==================== 

        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            selection = data[0];

            _KuenSelection_index = (int.Parse(bibleData.Book) - 1);
            OnPropertyChanged(nameof(KuenSelection_index));
            KuenChanged();

            _JangSelection = int.Parse(bibleData.Chapter);
            OnPropertyChanged(nameof(JangSelection));
            JangChanged();

            _JeulSelection = int.Parse(bibleData.Verse);
            OnPropertyChanged(nameof(JeulSelection));
        }

        public void ShowContent()
        {
            if (module.ProgramOption.BibleFramePath == null)
                    System.Windows.MessageBox.Show("성경 출력 틀ppt를 등록해주세요!", "ppt틀 등록되지 않음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
                new module.ShowStarter().BibleShowStart(
                    bibleData.Book
                    + bibleData.Chapter
                    + bibleData.Verse);
        }
    }
}
