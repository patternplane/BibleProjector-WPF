﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleControlViewModel : INotifyPropertyChanged
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

        public void hideBible()
        {
            SlideShowHide();
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
            BiblePages = new BindingList<string>(
                module.StringModifier.makeStringPage(
                    Database.getBible(Kjjeul)
                    ,module.ProgramOption.Bible_CharPerLine
                    , module.ProgramOption.Bible_LinePerSlide
                    )
                );
            setBibleSlide_BibleChapter(bible_display, chapter);
            CurrentPageIndex = 0;

            SlideShowRun();

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
        private BindingList<string> BiblePages_in;
        public BindingList<string> BiblePages { get { return BiblePages_in; } set { BiblePages_in = value; NotifyPropertyChanged(); } }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                    // 페이지 변경 처리
                    setBibleSlide_VerseContent(BiblePages[CurrentPageIndex_in], verse, CurrentPageIndex_in == 0);
                NotifyPropertyChanged();
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

        void SlideShowRun()
        {
            Powerpoint.Bible.SlideShowRun();
        }

        void SlideShowHide()
        {
            Powerpoint.Bible.SlideShowHide();
        }

        void setBibleSlide_VerseContent(string bibleContent,int verse, bool isFirstPage = false)
        {
            Powerpoint.Bible.Change_VerseContent(verse.ToString(), bibleContent, isFirstPage);
        }

        void setBibleSlide_BibleChapter(string bible, int chapter)
        {
            Powerpoint.Bible.Change_BibleChapter(bible, chapter.ToString());
        }

        void hideText()
        {
            Powerpoint.Bible.HideText();
        }

        void ShowText()
        {
            Powerpoint.Bible.ShowText();
        }

        void offDisplay()
        {
            Powerpoint.Bible.SlideShowHide();
            //Powerpoint.Bible.OffDisplay();
        }

        void onDisplay()
        {
            Powerpoint.Bible.SlideShowRun();
            //Powerpoint.Bible.OnDisplay();
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
