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
    class LyricViewModel : INotifyPropertyChanged
    {
        // 파일 입출력시 구분자
        private const string SEPARATOR = "∂";
        private const string RESERVER = "∇";
        // 검색창 기본 텍스트
        private const string DEFAUL_SEARCH_TEXT = "(가사 또는 제목으로 검색)";

        // ============================================ 세팅 ==============================================

        public LyricViewModel()
        {
            getData();
            /*
            if (LyricList == null)
                MessageBox.Show("가사 불러오기 실패!\n프로그램을 종료 후 다시 시작해주세요.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            */
        }

        void getData()
        {
            List<SingleLyric> PrimitiveLyricList = new List<SingleLyric>();

            string rawData = module.ProgramData.getLyricData(this).TrimEnd();

            foreach (string data in rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] line = data.TrimStart().Split(new string[] { "\r\n" }, 2, StringSplitOptions.None);
                if (line.Length == 1)
                    PrimitiveLyricList.Add(new SingleLyric(line[0], ""));
                else if (line.Length == 2)
                    PrimitiveLyricList.Add(new SingleLyric(line[0], line[1]));
            }

            List<LyricReserve> PrimitiveReserveList = getReserveData();
            foreach (LyricReserve r in PrimitiveReserveList)
                r.lyric = PrimitiveLyricList[r.lyricNumber];

            PrimitiveLyricList.Sort(delegate (SingleLyric a, SingleLyric b) { return a.title.CompareTo(b.title); });

            LyricList = (new BindingList<SingleLyric>(PrimitiveLyricList));
            LyricReserveList = (new BindingList<LyricReserve>(PrimitiveReserveList));
        }

        List<LyricReserve> getReserveData()
        {
            List<LyricReserve> PrimitiveReserveList = new List<LyricReserve>();

            string rawData = module.ProgramData.getLyricReserveData().TrimEnd();

            LyricReserve reserveData;
            foreach (string data in rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                reserveData = new LyricReserve(null);
                reserveData.lyricNumber = int.Parse(data);
                PrimitiveReserveList.Add(reserveData);
            }

            return PrimitiveReserveList;
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

        public string getSaveData_Reserve()
        {

            StringBuilder str = new StringBuilder(50).Clear();

            foreach (LyricReserve r in LyricReserveList)
            {
                str.Append(LyricList.IndexOf(r.lyric));
                str.Append(SEPARATOR);
            }

            return str.ToString();
        }

        // ============================================ 속성 ==============================================

        // 곡 삭제버튼 보이기
        private bool isDeleteButtonEnable_in;
        public bool isDeleteButtonEnable { get { return isDeleteButtonEnable_in; } set { isDeleteButtonEnable_in = value; NotifyPropertyChanged(); } }

        // 곡 추가버튼 보이기
        private bool isAddButtonEnable_in = true;
        public bool isAddButtonEnable { get { return isAddButtonEnable_in; } set { isAddButtonEnable_in = value; NotifyPropertyChanged(); } }

        // 가사 선택값
        private SingleLyric currentLyric_in;
        public SingleLyric currentLyric { get { return currentLyric_in; } set { currentLyric_in = value; SelectedLyric = currentLyric_in;
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
                NotifyPropertyChanged(); } }

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
        public BindingList<SingleLyric> LyricList { get; set; }

        /// <summary>
        /// 곡의 한 단위를 나타냅니다.
        /// </summary>
        public class SingleLyric :INotifyPropertyChanged
        {
            public SingleLyric(string title, string content)
            {
                this.title = title;
                this.content = content;
            }

            private string title_in;
            public String title { get { return title_in; } set { title_in = value; NotifyPropertyChanged(); } }
            private string content_in;
            public String content { get { return content_in; } set { content_in = value; NotifyPropertyChanged(); } }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

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

        // 곡 추가 제목
        public string AddLyricTitle { get; set; } = "";

        // 곡 추가 가사
        public string AddLyricContent { get; set; } = "";

        // 예약 단위
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
            public SingleLyric lyric { get { return lyric_in; }  set { lyric_in = value; NotifyPropertyChanged(); } }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        // 예약 리스트
        public BindingList<LyricReserve> LyricReserveList { get; set; }
        // 현재 (출력)선택값
        private LyricReserve LyricReserveSelection_in;
        public LyricReserve LyricReserveSelection { get { return LyricReserveSelection_in; } set { LyricReserveSelection_in = value;
                if (LyricReserveSelection_in != null)
                    SelectedLyric = LyricReserveSelection_in.lyric;
                else
                    SelectedLyric = null;
            } }

        // 출력 곡 선택값
        private SingleLyric SelectedLyric_in;
        public SingleLyric SelectedLyric { get { return SelectedLyric_in; } set
            {
                SelectedLyric_in = value;
                NotifyPropertyChanged();
            }
        }

        // ============================================ 이벤트에 쓰일 함수 ==============================================

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
                LyricList.Add(newLyric);
                if (lastSearchPattern != null)
                {
                    refreshSearchItem(newLyric, lastSearchPattern);
                    SearchText = lastSearchPattern;
                }
                currentLyric = newLyric;
            }
        }

        public void RunDelete()
        {
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

        // ============================================ 메소드 ==============================================

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
