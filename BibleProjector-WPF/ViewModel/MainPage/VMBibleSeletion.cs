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

        // ========= bible search bindings =========

        public bool ResultPopupOpen { get; set; } = false;
        public string SearchText { get; set; } = "";
        public bool IsSecondSearchButton { get; set; } = false;

        public bool IsMultiPage { get; set; } = false;
        public int PagePosition { get; set; }
        public int MaxPagePosition { get; set; }
        public string MovePageNumber
        {
            get { return ""; }
            set
            {
                int newVal;
                if (int.TryParse(value, out newVal)
                    && newVal >= 1 && newVal <= lastPage)
                    setPage(newVal);
            }
        }

        public BindingList<VMBiblePhraseSearchData> SearchResultList { get; set; } = new BindingList<VMBiblePhraseSearchData>() { };
        private VMBiblePhraseSearchData _SelectedSearchItem = null;
        private VMBiblePhraseSearchData lastSelectedSearchItem = null;
        public VMBiblePhraseSearchData SelectedSearchItem 
        { 
            get { return _SelectedSearchItem; }
            set 
            { 
                _SelectedSearchItem = value;
                if (SelectedSearchItem != null)
                {
                    lastSelectedSearchItem = SelectedSearchItem;

                    module.Data.BibleData data = (module.Data.BibleData)_SelectedSearchItem.getData();
                    selectBible(data.book, data.chapter, data.verse);
                    showPreviewItemEventManager.InvokeShowPreviewItem(_SelectedSearchItem);
                }
            } 
        }

        public ICommand CSearchBible { get; }
        public ICommand CMoveToPrevPage { get; }
        public ICommand CMoveToNextPage { get; }

        private VMBiblePhraseSearchData[] totalSearchResults;
        private int currentPage;
        private int lastPage;
        private static readonly int MAX_PAGE_SIZE = 100;

        // ========= fields =========

        private module.ReserveDataManager reserveDataManager;
        private module.ShowStarter showStarter;
        private module.BibleSearcher bibleSearcher;
        private Event.ShowPreviewItemEventManager showPreviewItemEventManager;

        public VMBibleSeletion(
            module.ReserveDataManager reserveDataManager,
            module.ShowStarter showStarter,
            Event.BibleSelectionEventManager bibleSelectionEventManager,
            Event.ShowPreviewItemEventManager showPreviewItemEventManager,
            Event.KeyInputEventManager keyInputEventManager,
            module.BibleSearcher bibleSearcher)
        {
            this.CBookSelection = new RelayCommand(obj => bookSelector((int)obj));
            this.CChapterSelection = new RelayCommand(obj => chapterSelector((int)obj));
            this.CVerseSelection = new RelayCommand(obj => verseSelector((int)obj));
            this.CShowBible = new RelayCommand(obj => startShow());
            this.CReserveBible = new RelayCommand(obj => reserveThis());

            this.CSearchBible = new RelayCommand(obj => searchBible());
            this.CMoveToPrevPage = new RelayCommand(obj => setPage(currentPage - 1));
            this.CMoveToNextPage = new RelayCommand(obj => setPage(currentPage + 1));

            this.reserveDataManager = reserveDataManager;
            this.showStarter = showStarter;
            this.bibleSearcher = bibleSearcher;

            this.showPreviewItemEventManager = showPreviewItemEventManager;

            bibleSelectionEventManager.BibleSelectionEvent += EH_BibleSelection;
            keyInputEventManager.KeyDown += keyInput;

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

        // ========== search bible ==========

        private string previousSearchText = "";
        private bool previousSearchHasBlank = false;
        private void searchBible()
        {
            if (SearchText == null || SearchText.Length < 1)
                return;
            if (previousSearchText.CompareTo(SearchText) != 0
                || previousSearchHasBlank != IsSecondSearchButton)
            {
                previousSearchText = SearchText;
                previousSearchHasBlank = IsSecondSearchButton;

                (string kjjeul, string content, (int startIdx, int lastIdx)[] pos)[] rawResult = bibleSearcher.getSearchResultbyPhrase(SearchText, !previousSearchHasBlank);
                totalSearchResults = new VMBiblePhraseSearchData[rawResult.Length];
                for (int i = 0; i < rawResult.Length; i++)
                    totalSearchResults[i] = new VMBiblePhraseSearchData(rawResult[i].kjjeul, rawResult[i].content, rawResult[i].pos);
                lastPage = (totalSearchResults.Length + MAX_PAGE_SIZE - 1) / MAX_PAGE_SIZE;

                setPage(1, true);

                MaxPagePosition = lastPage;
                OnPropertyChanged(nameof(MaxPagePosition));
                IsMultiPage = (lastPage > 1);
                OnPropertyChanged(nameof(IsMultiPage));
            }
            if (SearchResultList.Count > 0)
            {
                ResultPopupOpen = true;
                OnPropertyChanged(nameof(ResultPopupOpen));
            }
        }

        /// <summary>
        /// 검색 페이지를 이동합니다.
        /// <br/><paramref name="forceSetup"/>이 <see langword="true"/>이면 이미 해당 페이지여도 강제로 갱신합니다.
        /// <br/><paramref name="page"/>는 1부터 시작하는 범위를 갖습니다.
        /// </summary>
        /// <param name="page"></param>
        private void setPage(int page, bool forceSetup = false)
        {
            if (page > lastPage)
                page = lastPage;
            if (page < 1)
                page = 1;

            if (!forceSetup && page == currentPage)
                return;

            int firstIdx = (page - 1) * MAX_PAGE_SIZE;
            int lastIdx = Math.Min(totalSearchResults.Length - 1, page * MAX_PAGE_SIZE - 1);
            SearchResultList.Clear();
            while (firstIdx <= lastIdx)
                SearchResultList.Add(totalSearchResults[firstIdx++]);

            if (SearchResultList.Contains(lastSelectedSearchItem))
            {
                _SelectedSearchItem = lastSelectedSearchItem;
                OnPropertyChanged(nameof(SelectedSearchItem));
            }

            currentPage = page;
            PagePosition = page;
            OnPropertyChanged(nameof(PagePosition));
        }

        private void keyInput(Key key, bool isDown)
        {
            if (isDown
                && (key == Key.LeftShift
                || key == Key.RightShift))
            {
                IsSecondSearchButton = true;
                OnPropertyChanged(nameof(IsSecondSearchButton));
            }
            if (!isDown
                && (key == Key.LeftShift
                || key == Key.RightShift))
            {
                IsSecondSearchButton = false;
                OnPropertyChanged(nameof(IsSecondSearchButton));
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
            showPreviewItemEventManager.InvokeShowPreviewItem(new module.Data.BibleData(bookNumber, chapterNumber, verseNumber));
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
