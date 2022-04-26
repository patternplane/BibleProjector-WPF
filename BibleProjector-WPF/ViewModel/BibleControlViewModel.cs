using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleControlViewModel
    {
        // ================================================ 세팅 ================================================

        public BibleControlViewModel(string Kjjeul)
        {
            showBible(Kjjeul);
        }

        public void showBible(string Kjjeul)
        {
            this.Kjjeul = Kjjeul;
            newBibleSetting();
        }

        private void showBible_next()
        {
            Kjjeul = Database.getBibleIndex_Next(Kjjeul);
            newBibleSetting();
        }

        private void showBible_previous()
        {
            Kjjeul = Database.getBibleIndex_Previous(Kjjeul);
            newBibleSetting();
        }

        private void newBibleSetting()
        {
            setCurrentBibleInfo(bible_display, chapter, verse);

            BiblePages = null;
            BiblePages = module.StringModifier.makeStringPage(); // 구절을 어느 규격으로 재단할지 설정값도 반영해야함.
            setBibleSlide_BibleChapter(bible_display, chapter);
            CurrentPageIndex = 0;

            isDisplayShow = true;
            isTextShow = true;
        }

        string Kjjeul_in;
        string Kjjeul { get { return Kjjeul_in; } set { Kjjeul_in = value;
                bible_display = Database.getTitle(Kjjeul_in.Substring(0, 2));
                chapter = int.Parse(Kjjeul_in.Substring(2, 3));
                verse = int.Parse(Kjjeul_in.Substring(5, 3));
            } }
        string bible_display = null;
        int chapter = -1;
        int verse = -1;

        // ================================================ 속성 ================================================

        // 화면 끄기 토글버튼 - isChecked
        private bool isDisplayShow_in;
        public bool isDisplayShow { get { return isDisplayShow_in; } set { isDisplayShow_in = value;
                if (isDisplayShow_in)
                    onDisplay();
                else
                    offDisplay();
            } }
        // 자막 숨기기 토글버튼 - isChecked
        private bool isTextShow_in;
        public bool isTextShow { get { return isTextShow_in; } set { isTextShow_in = value;
                if (isTextShow_in)
                    ShowText();
                else
                    hideText();
            }
        }

        // 현재 성경 위치 표시 - Text
        public string CurrentBibleInfo { get; set; }

        // 성경 페이지 리스트박스
        public BindingList<string> BiblePages { get; set; }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                {
                    // 페이지 변경 처리
                    if (CurrentPageIndex_in == 0)
                        setBibleSlide_VerseContent(bibleContent:BiblePages[CurrentPageIndex_in]);
                    else
                        setBibleSlide_VerseContent(bibleContent: BiblePages[CurrentPageIndex_in], verse:verse);
                }
            } }

        // ================================================ 속성 메소드 ================================================

        void setCurrentBibleInfo(string book, int chapter, int verse)
        {
            CurrentBibleInfo = string.Format("{0}\r\n{1}장 {2}절",book,chapter,verse);
        }

        // ================================================ 이벤트에 쓰일 함수 ================================================

        public void RunNextPage()
        {
            if (CurrentPageIndex != BiblePages.Count - 1)
                CurrentPageIndex++;
            else
            {
                // 다음 구절로 이동처리
                showBible_next();
            }
        }

        public void RunPreviousPage()
        {
            if (CurrentPageIndex != 0)
                CurrentPageIndex--;
            else
            {
                // 이전 구절로 이동처리
                showBible_previous();
            }
        }

        // ================================================ 동작 ================================================

        void setBibleSlide_VerseContent(string bibleContent,int verse = -1)
        {
            // verse가 -1이면 절부분은 숨기기
        }

        void setBibleSlide_BibleChapter(string bible, int chapter)
        {

        }

        void hideText()
        {

        }

        void ShowText()
        {
            if (CurrentPageIndex == 0)
                // 절 보이기
                ;
            else
                // 절 숨기기
                ;
        }

        void offDisplay()
        {

        }

        void onDisplay()
        {
            // 화면 키고

            ShowText();
        }
    }
}
