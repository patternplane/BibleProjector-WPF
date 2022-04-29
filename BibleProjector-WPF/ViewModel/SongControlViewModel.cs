using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class SongControlViewModel : INotifyPropertyChanged
    {
        // ================================================ 세팅 ================================================

        public SongControlViewModel(string title, string[] content)
        {
            showSong(Kjjeul);
        }

        public void showSong()
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
            setCurrentSongInfo("test");

            SongPages = null;
            SongPages = new BindingList<string>(
                module.StringModifier.makeStringPage(
                    Database.getBible(Kjjeul)
                    ,module.ProgramOption.Bible_CharPerLine
                    ,module.ProgramOption.Bible_LinePerSlide
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

        // 현재 찬양곡 제목표시
        public string CurrentSongInfo { get; set; }

        // 찬양곡 페이지 리스트박스
        private BindingList<string> SongPages_in;
        public BindingList<string> SongPages { get { return SongPages_in; } set { SongPages_in = value; NotifyPropertyChanged(); } }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                {
                    // 페이지 변경 처리
                    if (CurrentPageIndex_in == 0)
                        ;//setBibleSlide_VerseContent(bibleContent:SongPages[CurrentPageIndex_in], verse: verse);
                    else
                        ;//setBibleSlide_VerseContent(bibleContent: SongPages[CurrentPageIndex_in]);
                }
                NotifyPropertyChanged();
            } }

        // ================================================ 속성 메소드 ================================================

        void setCurrentSongInfo(string title)
        {
            CurrentSongInfo = title;
        }

        // ================================================ 이벤트에 쓰일 함수 ================================================

        public void RunNextPage()
        {
            if (CurrentPageIndex != SongPages.Count - 1)
                CurrentPageIndex++;
        }

        public void RunPreviousPage()
        {
            if (CurrentPageIndex != 0)
                CurrentPageIndex--;
        }

        // ================================================ 동작 ================================================

        void SlideShowRun()
        {
            //Powerpoint.Song_SlideShowRun();
        }

        void setBibleSlide_Content(string bibleContent,int verse = -1)
        {
            if (verse == -1)
                ;//Powerpoint.Song_Change_VerseContent("", bibleContent);
            else
                ;// Powerpoint.Song_Change_VerseContent(verse.ToString(), bibleContent);
        }

        void setBibleSlide_BibleChapter(string bible, int chapter)
        {
            //Powerpoint.Song_Change_BibleChapter(bible, chapter.ToString());
        }

        void hideText()
        {
            //Powerpoint.Song_HideText();
        }

        void ShowText()
        {
            //Powerpoint.Song_ShowText();
        }

        void offDisplay()
        {
            //Powerpoint.Song_OffDisplay();
        }

        void onDisplay()
        {
            //Powerpoint.Song_OnDisplay();
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
