using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMBibleSeletion : ViewModel
    {
        private const int OldBookCount = 39;

        public BindingList<string> OldBookList { get; }
        public BindingList<string> NewBookList { get; }
        public BindingList<int> ChapterList { get; }
        public BindingList<int> VerseList { get; }

        public string BookDisplay { get; private set; }
        public string ChapterDisplay { get; private set; }
        public string VerseDisplay { get; private set; }

        public int BookSetter { get; set; }
        public int ChapterSetter { get; set; }
        public int VerseSetter { get; set; }

        private int bookNumber;
        private int chapterNumber;
        private int verseNumber;

        public ICommand CBookSelection { get; }
        public ICommand CChapterSelection { get; }
        public ICommand CVerseSelection { get; }
        public ICommand CShowBible { get; }
        public ICommand CReserveBible { get; }

        private module.ReserveDataManager reserveDataManager;
        private module.ShowStarter showStarter;

        public VMBibleSeletion(module.ReserveDataManager reserveDataManager, module.ShowStarter showStarter, Event.BibleSelectionEventManager bibleSelectionEventManager)
        {
            this.CBookSelection = new RelayCommand(obj => bookSelector((int)obj));
            this.CChapterSelection = new RelayCommand(obj => chapterSelector((int)obj));
            this.CVerseSelection = new RelayCommand(obj => verseSelector((int)obj));
            this.CShowBible = new RelayCommand(obj => startShow());
            this.CReserveBible = new RelayCommand(obj => reserveThis());

            this.reserveDataManager = reserveDataManager;
            this.showStarter = showStarter;

            bibleSelectionEventManager.BibleSelectionEvent += EH_BibleSelection;

            string[] ts = Database.getBibleTitles_string();
            this.OldBookList = new BindingList<string>();
            this.NewBookList = new BindingList<string>();

            for (int i = 0; i < OldBookCount; i++)
                OldBookList.Add(ts[i]);
            for (int i = OldBookCount; i < ts.Length; i++)
                NewBookList.Add(ts[i]);

            this.ChapterList = new BindingList<int>();
            this.VerseList = new BindingList<int>();
        }

        // ========== Methods ==========

        private bool availBibleSelection()
        {
            if (bookNumber < 1 || bookNumber > 66)
                return false;

            if (chapterNumber < 1 || chapterNumber > Database.getChapterCount(bookNumber.ToString("D2")))
                return false;

            if (verseNumber < 1 || verseNumber > Database.getVerseCount(bookNumber.ToString("D2") + chapterNumber.ToString("D3")))
                return false;

            return true;
        }

        private void selectBible(int bookNumber, int chapterNumber, int verseNumber)
        {
            setBook(bookNumber);
            setChapter(chapterNumber);
            setVerse(verseNumber);

            BookSetter = bookNumber;
            OnPropertyChanged(nameof(BookSetter));
            ChapterSetter = chapterNumber;
            OnPropertyChanged(nameof(ChapterSetter));
            VerseSetter = verseNumber;
            OnPropertyChanged(nameof(VerseSetter));
        }

        private void setBook(int bookNumber)
        {
            if (bookNumber >= 1 && bookNumber <= 66)
            {
                string kuen = bookNumber.ToString("D2");

                this.bookNumber = bookNumber;
                BookDisplay = Database.getTitle(kuen);
                OnPropertyChanged(nameof(BookDisplay));

                ChapterList.Clear();
                for (int i = 1, count = Database.getChapterCount(kuen); i <= count; i++)
                    ChapterList.Add(i);
            }
            else
            {
                this.bookNumber = -1;
                BookDisplay = "";
                OnPropertyChanged(nameof(BookDisplay));

                ChapterList.Clear();
            }

            this.chapterNumber = -1;
            ChapterDisplay = "";
            OnPropertyChanged(nameof(ChapterDisplay));

            this.verseNumber = -1;
            VerseDisplay = "";
            OnPropertyChanged(nameof(VerseDisplay));

            VerseList.Clear();
        }

        private void setChapter(int chapterNumber)
        {
            if (chapterNumber >= 1)
            {
                this.chapterNumber = chapterNumber;
                ChapterDisplay = chapterNumber.ToString();
                OnPropertyChanged(nameof(ChapterDisplay));

                VerseList.Clear();
                for (int i = 1, count = Database.getVerseCount(this.bookNumber.ToString("D2") + this.chapterNumber.ToString("D3")); i <= count; i++)
                    VerseList.Add(i);
            }
            else
            {
                this.chapterNumber = -1;
                ChapterDisplay = "";
                OnPropertyChanged(nameof(ChapterDisplay));

                VerseList.Clear();
            }

            this.verseNumber = -1;
            VerseDisplay = "";
            OnPropertyChanged(nameof(VerseDisplay));
        }

        private void setVerse(int verseNumber)
        {
            if (verseNumber >= 1)
            {
                this.verseNumber = verseNumber;
                VerseDisplay = verseNumber.ToString();
                OnPropertyChanged(nameof(VerseDisplay));
            }
            else
            {
                this.verseNumber = -1;
                VerseDisplay = "";
                OnPropertyChanged(nameof(VerseDisplay));
            }
        }

        // ========== Bible Selection Event Handler ==========

        private void EH_BibleSelection(int book, int chapter, int verse)
        {
            selectBible(book, chapter, verse);
        }

        // ========== Command Behavior ==========

        private void bookSelector(int bookNumber) {
            setBook(bookNumber);
        }

        private void chapterSelector(int chapterNumber)
        {
            setChapter(chapterNumber);
        }

        private void verseSelector(int verseNumber)
        {
            setVerse(verseNumber);
        }

        private void startShow()
        {
            if (availBibleSelection())
                showStarter.Show(new module.Data.BibleData(bookNumber, chapterNumber, verseNumber));
        }

        private void reserveThis()
        {
            if (availBibleSelection())
                reserveDataManager.AddReserveItem(this, new module.Data.BibleData(bookNumber, chapterNumber, verseNumber));
        }
    }
}
