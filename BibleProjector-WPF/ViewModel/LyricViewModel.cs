using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel
{
    // 데이터 규격

    /// <summary>
    /// 곡의 한 단위를 나타냅니다.
    /// </summary>
    public class SingleLyric : INotifyPropertyChanged
    {
        public SingleLyric() { }

        public SingleLyric(string title, string content)
        {
            this.title = title;
            this.content = content;
        }

        public String title { get { return title_get(); } set { title_set(value); NotifyPropertyChanged(); } }
        public String content { get { return content_get(); } set { content_set(value); NotifyPropertyChanged(); } }

        // title
        private string title_in;
        protected virtual string title_get()
        {
            return title_in;
        }
        protected virtual void title_set(string value)
        {
            title_in = value;
        }

        // content
        private string content_in;
        protected virtual string content_get()
        {
            return content_in;
        }
        protected virtual void content_set(string value)
        {
            content_in = value;
        }

        public int getIndexInList()
        {
            return LyricViewModel.LyricList.IndexOf(this);
        }

        // 출력
        public virtual string[][][] makeSongData(int linePerPage)
        {
            string[] pages = module.StringModifier.makePageWithLines(this.content, linePerPage);
            string[][][] songData = new string[pages.Length][][];

            for (int i = 0; i < pages.Length; i++)
            {
                songData[i] = new string[2][];

                songData[i][0] = new string[2];
                songData[i][0][0] = "{t}";
                songData[i][0][1] = this.title;

                songData[i][1] = new string[2];
                songData[i][1][0] = "{c}";
                songData[i][1][1] = pages[i];
            }

            return songData;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// 찬송가의 한 단위를 나타냅니다.
    /// </summary>
    public class SingleHymn : SingleLyric
    {
        public int VerseNum { get { return currentVerseNum(); } private set { } }

        public SingleHymn(string title, string[] content) : base()
        {
            this.title_set(title);
            Verse = content;
        }

        int currentVerse = 0;
        string[] Verse;

        // 절 작업
        public int currentVerseNum()
        {
            return currentVerse + 1; // 실제 절로 변환
        }
        public int VerseCount()
        {
            return Verse.Length;
        }
        public void setVerseNum(int verse)
        {
            verse--; // 인덱스로 변환

            if (verse < 0)
                currentVerse = 0;
            else if (verse >= Verse.Length)
                currentVerse = Verse.Length - 1;
            else
                currentVerse = verse;
        }
        public void setPreviousVerseNum()
        {
            if (currentVerse > 0)
                currentVerse--;
        }
        public void setNextVerseNum()
        {
            if (currentVerse < Verse.Length - 1)
                currentVerse++;
        }

        // content 접근
        protected override string content_get()
        {
            return Verse[currentVerse];
        }
        protected override void content_set(string value)
        {
            Verse[currentVerse] = value;
        }

        // 출력
        public override string[][][] makeSongData(int linePerPage)
        {
            string[] pages;
            List<string[][]> songData = new List<string[][]>(4);

            for (int i = 0; i < Verse.Length; i++)
            {
                pages = module.StringModifier.makePageWithLines(Verse[i], linePerPage);

                for (int j = 0; j < pages.Length; j++)
                {
                    songData.Add(new string[4][]);

                    songData.Last()[0] = new string[2];
                    songData.Last()[0][0] = "{t}";
                    songData.Last()[0][1] = this.title;

                    songData.Last()[1] = new string[2];
                    songData.Last()[1][0] = "{c}";
                    songData.Last()[1][1] = pages[j];

                    songData.Last()[2] = new string[2];
                    songData.Last()[2][0] = "{v}";
                        songData.Last()[2][1] = (j == 0)?
                            (i+1).ToString()
                            :"";

                    songData.Last()[3] = new string[2];
                    songData.Last()[3][0] = "{va}";
                        songData.Last()[3][1] = (i+1).ToString();
                }
            }

            return songData.ToArray();
        }
    }

    class LyricViewModel : INotifyPropertyChanged
    {
        // 파일 입출력시 구분자
        public const string SEPARATOR = "∂";
        private const string HYMN_SEPARATOR = "∫";
        // 검색창 기본 텍스트
        private const string DEFAUL_SEARCH_TEXT = "(가사 또는 제목으로 검색)";

        // ============================================ 세팅 ==============================================
        
        public LyricViewModel()
        {
            ReserveDataManager.subscriptToListChange(ListUpdate);

            getData();
            /*
            if (LyricList == null)
                MessageBox.Show("가사 불러오기 실패!\n프로그램을 종료 후 다시 시작해주세요.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            */
        }

        void getData()
        {
            List<SingleLyric> PrimitiveLyricList = getLyricList();
            List<SingleHymn> PrimitiveHymnList = getHymnList();

            PrimitiveLyricList.Sort(delegate (SingleLyric a, SingleLyric b) { return a.title.CompareTo(b.title); });
            LyricList = new BindingList<SingleLyric>(PrimitiveLyricList);
            HymnList = new BindingList<SingleHymn>(PrimitiveHymnList);
        }

        List<SingleLyric> getLyricList()
        {
            List<SingleLyric> PrimitiveLyricList = new List<SingleLyric>(10);

            string rawData = module.ProgramData.getLyricData(this).TrimEnd();

            foreach (string data in rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] line = data.TrimStart().Split(new string[] { "\r\n" }, 2, StringSplitOptions.None);
                if (line.Length == 1)
                    PrimitiveLyricList.Add(new SingleLyric(line[0], ""));
                else if (line.Length == 2)
                    PrimitiveLyricList.Add(new SingleLyric(line[0], line[1]));
            }

            return PrimitiveLyricList;
        }

        List<SingleHymn> getHymnList()
        {
            List<SingleHymn> PrimitiveHymnList = new List<SingleHymn>(10);

            string rawData = module.ProgramData.getHymnData();

            string[] songs = rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < songs.Length; i++) 
                PrimitiveHymnList.Add(
                    new SingleHymn(
                        (i + 1).ToString()
                        , songs[i].Split(new string[] { HYMN_SEPARATOR }, StringSplitOptions.None)
                        )
                    );   

            return PrimitiveHymnList;
        }

        // ============================================ 종료 및 저장 ==============================================

        public string getSaveData_Lyric()
        {
            StringBuilder str = new StringBuilder(50).Clear();

            foreach (SingleLyric lyric in LyricList)
            {
                str.AppendLine(lyric.title);
                str.Append(lyric.content);
                str.AppendLine(SEPARATOR);
            }

            return str.ToString();
        }

        public string getSaveData_Hymn()
        {
            StringBuilder str = new StringBuilder(50).Clear();

            foreach (SingleHymn hymn in HymnList)
            {
                hymn.setVerseNum(1);
                str.Append(hymn.content);
                for (int i = 2; i <= hymn.VerseCount();i++)
                {
                    str.Append(HYMN_SEPARATOR);
                    hymn.setVerseNum(i);
                    str.Append(hymn.content);
                }
                str.Append(SEPARATOR);
            }

            return str.ToString();
        }

        // ============================================ 속성 ==============================================

        // 가사 선택값
        private SingleLyric currentLyric_in;
        public SingleLyric currentLyric
        {
            get
            {
                return currentLyric_in;
            }
            set
            {
                currentLyric_in = value; SelectedLyric = currentLyric_in;
                isChangingLyric_title = true;
                isChangingLyric_content = true;
                if (currentLyric_in == null)
                {
                    isDeleteButtonEnable = false;
                    currentLyricTitle = "";
                    currentLyricContent = "";
                }
                else
                {
                    isDeleteButtonEnable = true;
                    currentLyricTitle = currentLyric_in.title;
                    currentLyricContent = currentLyric_in.content;
                }
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 곡 예약의 한 단위를 나타냅니다.
        /// </summary>
        public class LyricReserve : INotifyPropertyChanged
        {
            static int ReserveGenCount = 0;
            public LyricReserve(SingleLyric lyric)
            {
                this.personalNumber = ReserveGenCount++;
                this.lyric = lyric;
            }
            public int personalNumber;
            public int lyricNumber;
            private SingleLyric lyric_in;
            public SingleLyric lyric { get { return lyric_in; } set { lyric_in = value; NotifyPropertyChanged(); } }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        // =================================== 공용

        // 출력 곡 선택값
        private SingleLyric SelectedLyric_in;
        public SingleLyric SelectedLyric
        {
            get { return SelectedLyric_in; }
            set
            {
                if (value.GetType() == typeof(SingleHymn)) 
                {
                    if (module.ProgramOption.DefaultHymnFrame != null)
                        SongFrameSelection = module.ProgramOption.DefaultHymnFrame;
                }
                else if (module.ProgramOption.DefaultCCMFrame != null)
                    SongFrameSelection = module.ProgramOption.DefaultCCMFrame;
                SelectedLyric_in = value;
                NotifyPropertyChanged();
            }
        }

        // =================================== 곡 선택 탭

        // 곡 삭제버튼 보이기
        private bool isDeleteButtonEnable_in;
        public bool isDeleteButtonEnable { get { return isDeleteButtonEnable_in; } set { isDeleteButtonEnable_in = value; NotifyPropertyChanged(); } }

        // 곡 추가버튼 보이기
        private bool isAddButtonEnable_in = true;
        public bool isAddButtonEnable { get { return isAddButtonEnable_in; } set { isAddButtonEnable_in = value; NotifyPropertyChanged(); } }

        // 가사의 제목과 내용
        private bool needCheckNewLine_title = false;
        private bool isChangingLyric_title = false;
        private string currentLyricTitle_in;
        public string currentLyricTitle { get { return currentLyricTitle_in; } set {
                currentLyricTitle_in = value;
                if (!isChangingLyric_title)
                {
                    needCheckNewLine_title = true;
                    if (currentLyric != null)
                        currentLyric.title = currentLyricTitle_in;
                }
                else
                    needCheckNewLine_title = false;
                isChangingLyric_title = false;
                NotifyPropertyChanged();
            } }
        private bool needCheckNewLine_Content = false;
        private bool isChangingLyric_content = false;
        private string currentLyricContent_in;
        public string currentLyricContent
        {
            get { return currentLyricContent_in; }
            set
            {
                currentLyricContent_in = value;
                if (!isChangingLyric_content)
                {
                    needCheckNewLine_Content = true;
                    if (currentLyric != null)
                        currentLyric.content = currentLyricContent_in;
                }
                else
                    needCheckNewLine_Content = false;
                isChangingLyric_content = false;
                NotifyPropertyChanged();
            }
        }

        // 가사 리스트
        static public BindingList<SingleLyric> LyricList { get; set; }

        // 검색 문구
        private string SearchText_in = DEFAUL_SEARCH_TEXT;
        public string SearchText { get { return SearchText_in; } set { SearchText_in = value; NotifyPropertyChanged(); } }

        // 검색 선택값
        private searchPair currentSearchData_in;
        public searchPair currentSearchData { get { return currentSearchData_in; } set { currentSearchData_in = value; RunSetFromSearchValue(); NotifyPropertyChanged(); } }

        // 검색값 리스트
        private BindingList<searchPair> SearchList_in;
        public BindingList<searchPair> SearchList { get { return SearchList_in; } set { SearchList_in = value; NotifyPropertyChanged(); } }

        // 검색값 리스트 보이기
        private bool isSearchResultShow_in;
        public bool isSearchResultShow { get { return isSearchResultShow_in; } set { isSearchResultShow_in = value; NotifyPropertyChanged(); } }

        /// <summary>
        /// 검색 결과의 한 단위를 나타냅니다.
        /// </summary>
        public class searchPair
        {
            public String display { get; set; }
            public SingleLyric lyric { get; set; }
        }

        // =================================== 곡 추가 탭

        // 곡 추가 제목
        public string AddLyricTitle { get; set; } = "";

        // 곡 추가 가사
        public string AddLyricContent { get; set; } = "";

        // =================================== 찬송가 탭

        // 찬송가 리스트
        static public BindingList<SingleHymn> HymnList { get; set; }
        private SingleHymn HymnSelection_in;
        public SingleHymn HymnSelection
        {
            get
            {
                return HymnSelection_in;
            }
            set
            {
                HymnSelection_in = value;
                SelectedLyric = HymnSelection_in;

                BindingList<int> verselist = new BindingList<int>();
                for (int i = 1; i <= HymnSelection_in.VerseCount(); i++)
                    verselist.Add(i);
                VerseNumList = verselist;

                VerseNumSelection = 1;
            }
        }

        // 절 리스트
        public BindingList<int> VerseNumList { get; set; }
        private int VerseNumSelection_in;
        public int VerseNumSelection
        {
            get
            {
                return VerseNumSelection_in;
            }
            set
            {
                VerseNumSelection_in = value;

                HymnSelection.setVerseNum(VerseNumSelection_in);
                CurrentHymnPosition_Text = HymnSelection.title + "장 " + VerseNumSelection_in.ToString() + "절";

                VerseContent = HymnSelection.content;
            }
        }

        // 현재 선택위치 표시
        private string CurrentHymnPosition_Text_in;
        public string CurrentHymnPosition_Text 
        { 
            get { return CurrentHymnPosition_Text_in; }
            set {
                CurrentHymnPosition_Text_in = value;
                NotifyPropertyChanged();
            }
        }

        // 절 내용
        private string VerseContent_in = "";
        public string VerseContent
        {
            get { return VerseContent_in; }
            set
            {
                VerseContent_in = value;
                if (HymnSelection != null && HymnSelection.content.CompareTo(VerseContent_in) != 0)
                    HymnSelection.content = VerseContent_in;
                NotifyPropertyChanged();
            }
        }

        // =================================== 예약 탭

        // 예약 리스트
        ReserveCollectionUnit[] _LyricReserveList;
        public ReserveCollectionUnit[] LyricReserveList { get { return _LyricReserveList; } set { _LyricReserveList = value; NotifyPropertyChanged(nameof(LyricReserveList)); } }
        // 현재 (출력)선택값
        private ReserveCollectionUnit LyricReserveSelection_in;
        public ReserveCollectionUnit LyricReserveSelection { get { return LyricReserveSelection_in; } set { LyricReserveSelection_in = value;
                if (LyricReserveSelection_in != null)
                    SelectedLyric = ((module.SongReserveDataUnit)LyricReserveSelection_in.reserveData).lyric;
                else
                    SelectedLyric = null;
            }
        }

        ReserveCollectionUnit[] getSongReserveList()
        {
            return ReserveDataManager.instance.ReserveList.Where
                (obj => (obj.reserveType == module.ReserveType.Song)).ToArray();
        }


        void ListUpdate(object sender, Event.ReserveListChangedEventArgs e)
        {
            if ((e.changeType & Event.ReserveUpdateType.Song) > 0)
                LyricReserveList = getSongReserveList();
        }

        // =================================== 출력란

        // 슬라이드별 줄 수 
        private string LinePerSlideText_in = "2";
        public int LinePerSlide { get; private set; } = 2;
        public string LinePerSlideText { get { return LinePerSlideText_in; } set { LinePerSlideText_in = module.StringModifier.makeOnlyNum(value);
                if (LinePerSlideText_in.Length == 0)
                    LinePerSlide = 0;
                else
                    LinePerSlide = int.Parse(LinePerSlideText_in);
                NotifyPropertyChanged();
            } 
        }

        // ppt 틀
        public BindingList<module.SongFrameFile> SongFrameList { 
            get { return module.ProgramOption.SongFrameFiles; }
            set { }
        }
        public module.SongFrameFile SongFrameSelection { get; set; }

        // ============================================ 이벤트에 쓰일 함수 ==============================================

        public void RunShowLyric()
        {
            ShowLyric(SelectedLyric, SongFrameSelection, LinePerSlide);
        }

        public void RunSearch()
        {
            if (searchStart(SearchText))
            {
                if (lastSearchPattern != null)
                    SearchText = lastSearchPattern;
                if (SearchList.Count > 0)
                    isSearchResultShow = true;
            }
        }

        public void RunAdd()
        {
            SingleLyric newLyric = makeEnableLyric(AddLyricTitle, AddLyricContent);

            if (newLyric != null)
            {
                insertInOrder(newLyric);
                if (lastSearchPattern != null)
                {
                    refreshSearchItem(newLyric, lastSearchPattern);
                    SearchText = lastSearchPattern;
                }
                currentLyric = newLyric;
            }
        }
        void insertInOrder(SingleLyric newLyric) 
        {
            int i = 0;
            for(; i < LyricList.Count; i++)
                if (newLyric.title.CompareTo(LyricList[i].title) <= 0)
                {
                    LyricList.Insert(i, newLyric);
                    break;
                }
            if (i == LyricList.Count)
                LyricList.Insert(i, newLyric);
        }

        public void RunDelete()
        {
            if (MessageBox.Show("현재 선택된 곡을 삭제하시겠습니까?", "찬양곡 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            SingleLyric deleteItem = currentLyric;
            if (deleteLyric(currentLyric))
            {
                if (lastSearchPattern != null)
                {
                    refreshSearchItem(deleteItem, lastSearchPattern,true);
                    currentSearchData = null;
                    SearchText = lastSearchPattern;
                }
            }

            if (deleteItem == outedLyric)
                new module.ControlWindowManager().closeSongControl();
        }

        public void RunSetFromSearchValue()
        {
            if (currentSearchData != null && currentLyric != currentSearchData.lyric)
                currentLyric = currentSearchData.lyric;
        }

        public void RunCompleteModify()
        {
            if (needCheckNewLine_title)
            {
                currentLyricTitle = module.StringModifier.makeCorrectNewline(currentLyricTitle);
                needCheckNewLine_title = false;
            }
            if (needCheckNewLine_Content)
            {
                currentLyricContent = module.StringModifier.makeCorrectNewline(currentLyricContent);
                needCheckNewLine_Content = false;
            }
            
            if (lastSearchPattern != null)
                refreshSearchItem(currentLyric, lastSearchPattern);
        }

        public void RunApplyHymnModify()
        {
            VerseContent = module.StringModifier.makeCorrectNewline(VerseContent);
        }

        SingleLyric outedLyric = null;
        public void currentLyricOuted()
        {
            outedLyric = SelectedLyric;
        }

        public void RunAddReserveFromSelection()
        {
            if (SelectedLyric != null)
            {
                //LyricReserveList.Add(new LyricReserve(SelectedLyric));

                // 예약창에 보내는 데이터
                ViewModel.ReserveManagerViewModel.instance.ReserveDataManager.addReserve(
                    new module.SongReserveDataUnit(SelectedLyric));
            }
        }

        // ============================================ 메소드 ==============================================

        void ShowLyric(SingleLyric lyric, module.SongFrameFile FrameFile, int linePerSlide)
        {
            if (FrameFile == null)
                MessageBox.Show("찬양 출력 틀ppt를 등록해주세요!", "ppt 틀 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (lyric == null)
                MessageBox.Show("출력할 찬양곡을 선택해주세요!", "찬양곡 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                new module.ShowStarter().SongShowStart(
                    lyric.makeSongData(linePerSlide)
                    , FrameFile.Path
                    , lyric.GetType() == typeof(ViewModel.SingleHymn));
                currentLyricOuted();
            }
        }

        SingleLyric makeEnableLyric(String title, String content)
        {
            // 빈 제목
            if (title.Trim().Length == 0)
            {
                MessageBox.Show("제목을 입력해주세요!", "곡 추가 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            // 동일한 제목의 곡 존재
            foreach (SingleLyric t in LyricList)
                if (t.title.CompareTo(title) == 0)
                {
                    MessageBox.Show("동일한 제목의 곡이 있습니다!", "곡 추가 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

            return (new SingleLyric(module.StringModifier.makeCorrectNewline(title), module.StringModifier.makeCorrectNewline(content)));
        }

        bool deleteLyric(SingleLyric item)
        {
            if (item != null)
            {
                LyricList.Remove(item);
                return true;
            }
            return false;
        }

        private string lastSearchPattern;
        private void searchStart()
        {
            if (lastSearchPattern != null)
            {
                SearchList = null;
                SearchList = search(lastSearchPattern);
            }
        }
        private bool searchStart(String searchText)
        {
            if (searchText.Trim().Length > 0 && searchText.CompareTo(DEFAUL_SEARCH_TEXT) != 0)
            {
                SearchList = null;
                SearchList = search(searchText);
                lastSearchPattern = searchText;

                return true;
            }
            else
            {
                MessageBox.Show("검색할 내용을 입력하세요!", "검색값", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
            
        }

        /// <summary>
        /// 전체 곡에서 주어진 패턴으로 제목 또는 가사를 검색합니다.
        /// </summary>
        /// <param name="pattern">
        /// 찾을 패턴
        /// </param>
        /// <returns>
        /// 찾은 패턴값과 곡의 인덱스 정보가 담긴 searchPair 리스트
        /// </returns>
        BindingList<searchPair> search(String pattern)
        {
            BindingList<searchPair> searchList = new BindingList<searchPair>();

            int[] findPos;
            for (int i = 0; i < LyricList.Count; i++)
            {
                if (module.StringKMP.HasPattern(LyricList[i].title, pattern, delegate (char a, char b) { return a == b; }, false))
                    searchList.Add(new searchPair() { display = LyricList[i].title, lyric = LyricList[i] });

                findPos = module.StringKMP.FindPattern(LyricList[i].content, pattern, delegate (char a, char b) { return a == b; }, false);
                if (findPos.Length > 0)
                    foreach (String s in module.StringKMP.makeResultPreview(findPos, LyricList[i].content, pattern.Length))
                        searchList.Add(new searchPair() { display = "(" + LyricList[i].title + ") " + s, lyric = LyricList[i] });
            }

            return searchList;
        }

        searchPair[] search_OneLyric(String pattern, SingleLyric lyric)
        {
            List<searchPair> searchList = new List<searchPair>();

            if (module.StringKMP.HasPattern(lyric.title, pattern, delegate (char a, char b) { return a == b; }, false))
                searchList.Add(new searchPair() { display = lyric.title, lyric = lyric });

            int[] findPos = module.StringKMP.FindPattern(lyric.content, pattern, delegate (char a, char b) { return a == b; }, false);
            if (findPos.Length > 0)
                foreach (String s in module.StringKMP.makeResultPreview(findPos, lyric.content, pattern.Length))
                    searchList.Add(new searchPair() { display = "(" + lyric.title + ") " + s, lyric = lyric });

            return searchList.ToArray();
        }

        void refreshSearchItem(SingleLyric LyricItem,string searchPattern,bool onlyDelete = false)
        {
            if (onlyDelete)
            {
                for (int i = 0; i < SearchList.Count; i++)
                    if (SearchList[i].lyric == LyricItem)
                    {
                        for (; i < SearchList.Count && SearchList[i].lyric == LyricItem;)
                            SearchList.RemoveAt(i);
                        return;
                    }
            }
            else
            {
                bool isNewLyric = true;
                for (int i = 0; i < SearchList.Count; i++)
                {
                    if (LyricList.IndexOf(SearchList[i].lyric) == LyricList.IndexOf(LyricItem)) {
                        isNewLyric = false;
                        for (; i < SearchList.Count && SearchList[i].lyric == LyricItem;)
                            SearchList.RemoveAt(i);
                        currentSearchData = null;
                    }

                    if (i == SearchList.Count)
                        break;
                    else if (LyricList.IndexOf(SearchList[i].lyric) > LyricList.IndexOf(LyricItem))
                    {
                        foreach (searchPair p in search_OneLyric(searchPattern, LyricItem))
                        {
                            SearchList.Insert(i++, p);
                            if (!isNewLyric && currentSearchData == null)
                                currentSearchData = p;
                        }

                        return;
                    }
                }

                foreach (searchPair p in search_OneLyric(searchPattern, LyricItem))
                {
                    SearchList.Add(p);
                    if (!isNewLyric && currentSearchData == null)
                        currentSearchData = p;
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
