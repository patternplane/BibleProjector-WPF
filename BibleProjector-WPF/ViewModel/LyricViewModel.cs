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
    class LyricViewModel : ViewModel
    {

        // ============================================ 세팅 ==============================================

        module.ShowStarter showStarter;
        module.Data.SongManager songManager;
        module.ReserveDataManager reserveDataManager;

        public LyricViewModel(module.ShowStarter showStarter, module.Data.SongManager songManager, module.ReserveDataManager reserveDataManager)
        {
            this.showStarter = showStarter;
            this.songManager = songManager;
            this.reserveDataManager = reserveDataManager;

            LyricList = new BindingList<module.Data.SongData>(this.songManager.CCMs);
            HymnList = new BindingList<module.Data.SongData>(this.songManager.Hymns);
        }

        // ============================================ 속성 ==============================================

        // 가사 선택값
        /// <summary>
        /// 변경시 <see cref="setCurrentLyric"/>을 사용할 것
        /// </summary>
        private module.Data.SongData currentLyric_in;
        /// <summary>
        /// 변경시 <see cref="setCurrentLyric"/>을 사용할 것
        /// </summary>
        public module.Data.SongData currentLyric
        {
            get { return currentLyric_in; }
            set { setCurrentLyric(value, true); }
        }

        // =================================== 공용

        // 출력 곡 선택값
        private module.Data.SongData SelectedLyric_in;
        public module.Data.SongData SelectedLyric
        {
            get { return SelectedLyric_in; }
            set
            {
                if (value != null)
                {
                    module.Data.SongDataTypeEnum songType = value.songType;
                    if (songType == module.Data.SongDataTypeEnum.CCM
                        && module.ProgramOption.DefaultCCMFrame != null)
                        SongFrameSelection = module.ProgramOption.DefaultCCMFrame;
                    else if (songType == module.Data.SongDataTypeEnum.HYMN
                        && module.ProgramOption.DefaultHymnFrame != null)
                        SongFrameSelection = module.ProgramOption.DefaultHymnFrame;
                    else
                        SongFrameSelection = null;
                }
                SelectedLyric_in = value;
                OnPropertyChanged();
            }
        }

        // =================================== 곡 선택 탭

        // 곡 삭제버튼 보이기
        private bool isDeleteButtonEnable_in;
        public bool isDeleteButtonEnable { get { return isDeleteButtonEnable_in; } set { isDeleteButtonEnable_in = value; OnPropertyChanged(); } }

        // 곡 추가버튼 보이기
        private bool isAddButtonEnable_in = true;
        public bool isAddButtonEnable { get { return isAddButtonEnable_in; } set { isAddButtonEnable_in = value; OnPropertyChanged(); } }

        // 가사의 중복엔터 삭제 버튼 보이기
        public bool isMultiLineDeleteButtonEnable { get; set; } = false;

        // 가사의 제목과 내용
        /// <summary>
        /// 변경시 <see cref="setCurrentTitle"/>을 사용할 것
        /// </summary>
        private string currentLyricTitle_in;
        /// <summary>
        /// 변경시 <see cref="setCurrentTitle"/>을 사용할 것
        /// </summary>
        public string currentLyricTitle
        { 
            get { return currentLyricTitle_in; } 
            set { setCurrentTitle(value, true); }
        }
        /// <summary>
        /// 변경시 <see cref="setCurrentContent"/>을 사용할 것
        /// </summary>
        private string currentLyricContent_in;
        /// <summary>
        /// 변경시 <see cref="setCurrentContent"/>을 사용할 것
        /// </summary>
        public string currentLyricContent
        {
            get { return currentLyricContent_in; }
            set { setCurrentContent(module.StringModifier.makeCorrectNewline(value)); currentLyricContent_isUpdated = true; }
        }
        private bool currentLyricContent_isUpdated = false;
        public void submitModifyingCurrentContent()
        {
            if (currentLyricContent_isUpdated)
            {
                setCurrentContent(module.StringModifier.makeCorrectNewline(currentLyricContent_in), true);
                currentLyricContent_isUpdated = false;
            }
        }

        // 가사 리스트
        static public BindingList<module.Data.SongData> LyricList { get; set; }

        // 검색창 기본 텍스트
        private const string DEFAUL_SEARCH_TEXT = "(가사 또는 제목으로 검색)";

        // 검색 문구
        private string SearchText_in = DEFAUL_SEARCH_TEXT;
        public string SearchText { get { return SearchText_in; } set { SearchText_in = value; OnPropertyChanged(); } }

        // 검색 선택값
        private searchPair currentSearchData_in;
        public searchPair currentSearchData { get { return currentSearchData_in; } set { currentSearchData_in = value; RunSetFromSearchValue(); OnPropertyChanged(); } }

        // 검색값 리스트
        private BindingList<searchPair> SearchList_in;
        public BindingList<searchPair> SearchList { get { return SearchList_in; } set { SearchList_in = value; OnPropertyChanged(); } }

        // 검색값 리스트 보이기
        private bool isSearchResultShow_in;
        public bool isSearchResultShow { get { return isSearchResultShow_in; } set { isSearchResultShow_in = value; OnPropertyChanged(); } }

        /// <summary>
        /// 검색 결과의 한 단위를 나타냅니다.
        /// </summary>
        public class searchPair
        {
            public String display { get; set; }
            public module.Data.SongData lyric { get; set; }
        }

        // =================================== 곡 추가 탭

        // 곡 추가 제목
        public string AddLyricTitle { get; set; } = "";

        // 곡 추가 가사
        public string AddLyricContent { get; set; } = "";

        // =================================== 찬송가 탭

        // 찬송가 리스트
        static public BindingList<module.Data.SongData> HymnList { get; set; }
        private module.Data.SongData HymnSelection_in;
        public module.Data.SongData HymnSelection
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
                for (int i = 1; i <= HymnSelection_in.songContent.lyricCount; i++)
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

                CurrentHymnPosition_Text = HymnSelection.songTitle + "장 " + VerseNumSelection_in.ToString() + "절";

                VerseContent = HymnSelection.songContent.getContentByVerse(VerseNumSelection_in - 1);
            }
        }

        // 현재 선택위치 표시
        private string CurrentHymnPosition_Text_in;
        public string CurrentHymnPosition_Text 
        { 
            get { return CurrentHymnPosition_Text_in; }
            set {
                CurrentHymnPosition_Text_in = value;
                OnPropertyChanged();
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
                if (HymnSelection != null && HymnSelection.songContent.getContentByVerse(VerseNumSelection - 1).CompareTo(VerseContent_in) != 0)
                    HymnSelection.songContent.setContent(VerseContent_in, VerseNumSelection - 1);
                OnPropertyChanged();
            }
        }

        // =================================== 출력란

        // 슬라이드별 줄 수 
        private string LinePerSlideText_in = module.ProgramOption.Song_LinePerSlide.ToString();
        public int LinePerSlide { get; private set; } = 2;
        public string LinePerSlideText { get { return LinePerSlideText_in; } set { LinePerSlideText_in = module.StringModifier.makeOnlyNum(value);
                if (LinePerSlideText_in.Length == 0)
                    LinePerSlide = 0;
                else
                    LinePerSlide = int.Parse(LinePerSlideText_in);
                OnPropertyChanged();
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
            module.Data.SongData newLyric = makeEnableLyric(AddLyricTitle, AddLyricContent);

            if (newLyric != null)
            {
                songManager.AddSongInOrder(newLyric);

                if (lastSearchPattern != null)
                {
                    refreshSearchItem(newLyric, lastSearchPattern);
                    SearchText = lastSearchPattern;
                }
                setCurrentLyric(newLyric);
            }
        }

        public void RunDelete()
        {
            if (currentLyric == null)
                return;

            module.Data.SongData deleteItem = currentLyric;
            if (MessageBox.Show("현재 선택된 곡을 삭제하시겠습니까?", "찬양곡 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            songManager.DeleteSongItem(deleteItem);

            if (lastSearchPattern != null)
            {
                refreshSearchItem(deleteItem, lastSearchPattern,true);
                currentSearchData = null;
                SearchText = lastSearchPattern;
            }

            setCurrentLyric(null);
        }

        public void RunSetFromSearchValue()
        {
            if (currentSearchData != null && currentLyric != currentSearchData.lyric)
                setCurrentLyric(currentSearchData.lyric);
        }

        public void RunApplyHymnModify()
        {
            VerseContent = module.StringModifier.makeCorrectNewline(VerseContent);

            if (HymnSelection != null)
            {
                if (HymnSelection.songType == module.Data.SongDataTypeEnum.CCM)
                {
                    songManager.saveCCMData(false);
                    module.ProgramData.writeErrorLog("찬송가 수정 메소드 내에서 ccm 데이터가 변경됨! (LyricViewModel.RunApplyHymnModify)");
                }
                if (HymnSelection.songType == module.Data.SongDataTypeEnum.HYMN)
                    songManager.saveHymnData(false);
            }
        }

        public void RunAddReserveFromSelection()
        {
            if (SelectedLyric != null)
                reserveDataManager.AddReserveItem(this, SelectedLyric);
        }

        public void RunRemoveDoubleEnter()
        {
            if (MessageBox.Show("중복 개행을 삭제합니다!", "중복 개행 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Warning)
                == MessageBoxResult.OK)
            {
                setCurrentContent(module.StringModifier.RemoveMultiLinefeeds(currentLyricContent).Trim(), true);
            }
        }

        // ============================================ 메소드 ==============================================

        /// <summary>
        /// 현재 표시중인 가사를 설정합니다.
        /// <br/><paramref name="byBinding"/>이 <see langword="true"/>이면 UI에 의한 입력의 처리를 함께 진행합니다.
        /// </summary>
        /// <param name="lyric"></param>
        /// <param name="byBinding"></param>
        private void setCurrentLyric(module.Data.SongData lyric, bool byBinding = false)
        {
            currentLyric_in = lyric;
            if (!byBinding)
            {
                OnPropertyChanged(nameof(currentLyric));
            }

            SelectedLyric = currentLyric_in;

            if (currentLyric_in == null)
            {
                isDeleteButtonEnable = false;
                setCurrentTitle("");
                setCurrentContent("");
            }
            else
            {
                isDeleteButtonEnable = true;
                setCurrentTitle(currentLyric_in.songTitle);
                setCurrentContent(currentLyric_in.songContent.getRawContent());
            }
        }

        /// <summary>
        /// 현재 표시중인 곡의 제목을 설정합니다.
        /// <br/><paramref name="applyChange"/>이 <see langword="true"/>이면 변경을 반영하는 처리를 함께 진행합니다.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="applyChange"></param>
        private void setCurrentTitle(string title, bool applyChange = false)
        {
            if (applyChange)
            {
                currentLyricTitle_in = module.StringModifier.makeCorrectNewline(title);
                if (currentLyric != null)
                {
                    currentLyric.songTitle = currentLyricTitle_in;

                    if (currentLyric.songType == module.Data.SongDataTypeEnum.CCM)
                        songManager.saveCCMData(false);
                    if (currentLyric.songType == module.Data.SongDataTypeEnum.HYMN)
                        songManager.saveHymnData(false);

                    if (lastSearchPattern != null)
                        refreshSearchItem(currentLyric, lastSearchPattern);
                }
            }
            else
            {
                currentLyricTitle_in = title;
            }
            OnPropertyChanged(nameof(currentLyricTitle));
        }

        /// <summary>
        /// 현재 표시중인 곡의 가사를 설정합니다.
        /// <br/><paramref name="applyChange"/>이 <see langword="true"/>이면 변경을 반영하는 처리를 함께 진행합니다.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="applyChange"></param>
        private void setCurrentContent(string content, bool applyChange = false)
        {
            if (applyChange)
            {
                currentLyricContent_in = module.StringModifier.makeCorrectNewline(content);
                if (currentLyric != null)
                {
                    currentLyric.songContent.setContent(currentLyricContent_in, 0);

                    if (currentLyric.songType == module.Data.SongDataTypeEnum.CCM)
                        songManager.saveCCMData(false);
                    if (currentLyric.songType == module.Data.SongDataTypeEnum.HYMN)
                        songManager.saveHymnData(false);

                    if (lastSearchPattern != null)
                        refreshSearchItem(currentLyric, lastSearchPattern);
                }
            }
            else
            {
                currentLyricContent_in = content;
            }
            OnPropertyChanged(nameof(currentLyricContent));

            isMultiLineDeleteButtonEnable = currentLyricContent_in.Contains("\r\n\r\n");
            OnPropertyChanged(nameof(isMultiLineDeleteButtonEnable));
        }

        void ShowLyric(module.Data.SongData lyric, module.SongFrameFile FrameFile, int linePerSlide)
        {
            if (FrameFile == null)
                MessageBox.Show("찬양 출력 틀ppt를 등록해주세요!", "ppt 틀 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (lyric == null)
                MessageBox.Show("출력할 찬양곡을 선택해주세요!", "찬양곡 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                lyric.pptFrameFullPath = FrameFile.Path;
                lyric.linePerSlide = linePerSlide;
                showStarter.Show(lyric);
            }
        }

        module.Data.SongData makeEnableLyric(String title, String content)
        {
            // 빈 제목
            if (title.Trim().Length == 0)
            {
                MessageBox.Show("제목을 입력해주세요!", "곡 추가 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            // 동일한 제목의 곡 존재
            foreach (module.Data.SongData t in LyricList)
                if (t.songTitle.CompareTo(title) == 0)
                {
                    MessageBox.Show("동일한 제목의 곡이 있습니다!", "곡 추가 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

            return 
                new module.Data.SongData(
                    module.StringModifier.makeCorrectNewline(title), 
                    new module.Data.SongContent(module.StringModifier.makeCorrectNewline(content)),
                    module.Data.SongDataTypeEnum.CCM,
                    null);
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
                if (module.StringKMP.HasPattern(LyricList[i].songTitle, pattern, module.StringKMP.IgnoreCaseStringCompareFunc, false))
                    searchList.Add(new searchPair() { display = LyricList[i].songTitle, lyric = LyricList[i] });

                findPos = module.StringKMP.FindPattern(LyricList[i].songContent.getContentByVerse(0), pattern, module.StringKMP.IgnoreCaseStringCompareFunc, false);
                if (findPos.Length > 0)
                    foreach (String s in module.StringKMP.makeResultPreview(findPos, LyricList[i].songContent.getContentByVerse(0), pattern.Length))
                        searchList.Add(new searchPair() { display = "(" + LyricList[i].songTitle + ") " + s, lyric = LyricList[i] });
            }

            return searchList;
        }

        searchPair[] search_OneLyric(String pattern, module.Data.SongData lyric)
        {
            List<searchPair> searchList = new List<searchPair>();

            if (module.StringKMP.HasPattern(lyric.songTitle, pattern, module.StringKMP.IgnoreCaseStringCompareFunc, false))
                searchList.Add(new searchPair() { display = lyric.songTitle, lyric = lyric });

            int[] findPos = module.StringKMP.FindPattern(lyric.songContent.getContentByVerse(0), pattern, module.StringKMP.IgnoreCaseStringCompareFunc, false);
            if (findPos.Length > 0)
                foreach (String s in module.StringKMP.makeResultPreview(findPos, lyric.songContent.getContentByVerse(0), pattern.Length))
                    searchList.Add(new searchPair() { display = "(" + lyric.songTitle + ") " + s, lyric = lyric });

            return searchList.ToArray();
        }

        void refreshSearchItem(module.Data.SongData LyricItem,string searchPattern,bool onlyDelete = false)
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
    }
}
