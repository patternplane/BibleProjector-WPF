using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class ReadingControlViewModel : INotifyPropertyChanged
    {
        // ================================================ 세팅 ================================================

        public ReadingControlViewModel(int ReadingNumber)
        {
            showReading(ReadingNumber);
        }

        public void showReading(int ReadingNumber)
        {
            this.ReadingNumber = ReadingNumber;
            newReadingSetting();
        }

        public void hideReading()
        {
            SlideShowHide();
        }

        private void newReadingSetting()
        {
            setCurrentReadingInfo(ReadingNumber);

            ReadingPages = null;
            ReadingPages = new BindingList<string>(Database.getReadingContent(ReadingNumber));
            setReadingSlide_Title(Database.getReadingTitle(ReadingNumber));
            CurrentPageIndex = 0;

            isTextShow = true;
            isDisplayShow = true;
        }

        int ReadingNumber = -1;

        // ================================================ 속성 ================================================

        // 화면 끄기 토글버튼 - isChecked
        private bool isDisplayShow_in;
        public bool isDisplayShow { get { return isDisplayShow_in; } set { isDisplayShow_in = value;
                if (isDisplayShow_in)
                    onDisplay();
                else
                    offDisplay();
                NotifyPropertyChanged();
            } 
        }
        // 자막 숨기기 토글버튼 - isChecked
        private bool isTextShow_in;
        public bool isTextShow { get { return isTextShow_in; } set { isTextShow_in = value;
                if (isTextShow_in)
                    ShowText();
                else
                    hideText();
                NotifyPropertyChanged();
            }
        }

        // 현재 성경 위치 표시 - Text
        public string CurrentReadingInfo { get; set; }

        // 성경 페이지 리스트박스
        private BindingList<string> ReadingPages_in;
        public BindingList<string> ReadingPages { get { return ReadingPages_in; } set { ReadingPages_in = value; NotifyPropertyChanged(); } }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                {
                    // 페이지 변경 처리
                    setReadingSlide_Content(ReadingPages[CurrentPageIndex_in]);
                }
                NotifyPropertyChanged();
            } }

        // ================================================ 속성 메소드 ================================================

        void setCurrentReadingInfo(int ReadingNumber)
        {
            CurrentReadingInfo = Database.getReadingTitle(ReadingNumber);
        }

        // ================================================ 이벤트에 쓰일 함수 ================================================

        public void RunNextPage()
        {
            if (CurrentPageIndex != ReadingPages.Count - 1)
                CurrentPageIndex++;
        }

        public void RunPreviousPage()
        {
            if (CurrentPageIndex != 0)
                CurrentPageIndex--;
        }

        public void RunTopMost()
        {
            Powerpoint.Reading.TopMost();
        }

        // ================================================ 동작 ================================================

        void SlideShowRun()
        {
            Powerpoint.Reading.SlideShowRun();
        }

        void SlideShowHide()
        {
            Powerpoint.Reading.SlideShowHide();
        }

        void setReadingSlide_Content(string ReadingContent)
        {
            Powerpoint.Reading.Change_Content(ReadingContent);
        }

        void setReadingSlide_Title(string title)
        {
            Powerpoint.Reading.Change_Title(title);
        }

        void hideText()
        {
            Powerpoint.Reading.HideText();
        }

        void ShowText()
        {
            Powerpoint.Reading.ShowText();
        }

        void offDisplay()
        {
            Powerpoint.Reading.SlideShowHide();
            //Powerpoint.Reading.OffDisplay();
        }

        void onDisplay()
        {
            Powerpoint.Reading.SlideShowRun();
            //Powerpoint.Reading.OnDisplay();
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
