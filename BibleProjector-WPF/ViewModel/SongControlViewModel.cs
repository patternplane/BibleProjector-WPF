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

        public SongControlViewModel(string[][][] songData, string FrameFileName, bool isHymn)
        {
            // 반드시 songData는 모든 슬라이드마다 0. 제목, 1. 가사 일 것!
            // songData 규격 : [슬라이드 번호][정보 종류][커맨드(0)냐 내용(1)이냐]
            showSong(songData, FrameFileName, isHymn);
        }

        public void showSong(string[][][] songData, string FrameFileName, bool isHymn)
        {
            this.currentPPTName = FrameFileName;
            this.songData = songData;
            newSongSetting(isHymn);
        }

        public void hideSong()
        {
            SlideShowHide();
        }

        private void newSongSetting(bool isHymn)
        {
            setCurrentSongInfo(songData[0][0][1]);
            SetSongData(songData);

            SongPages = null;
            SongPages = new BindingList<string>();
            if (isHymn)
                foreach (string[][] data in songData)
                {
                    if (data[2][1].Length == 0)
                        SongPages.Add(data[1][1]);
                    else 
                        SongPages.Add("    " + data[2][1] + " 절\r\n\r\n" + data[1][1]);
                }
            else
                foreach (string[][] data in songData)
                    SongPages.Add(data[1][1]);
            CurrentPageIndex = 0;

            isTextShow = true;
            isDisplayShow = true;
        }

        string[][][] songData;
        string currentPPTName;
        public bool isSameFrame(string FrameFileName)
        {
            return (currentPPTName.CompareTo(FrameFileName) == 0);
        }

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

        // 현재 찬양곡 제목표시
        public string CurrentSongInfo { get; set; }

        // 찬양곡 페이지 리스트박스
        private BindingList<string> SongPages_in;
        public BindingList<string> SongPages { get { return SongPages_in; } set { SongPages_in = value; NotifyPropertyChanged(); } }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                    // 페이지 변경 처리
                    ChangePage(CurrentPageIndex_in);
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

        public void RunTopMost()
        {
            Powerpoint.Song.TopMost(currentPPTName);
        }

        // ================================================ 동작 ================================================

        void SetSongData(string[][][] songData)
        {
            Powerpoint.Song.SetSongData(currentPPTName,songData);
        }

        void SlideShowRun()
        {
            Powerpoint.Song.SlideShowRun(currentPPTName);
        }

        void SlideShowHide()
        {
            Powerpoint.Song.SlideShowHide(currentPPTName);
        }

        void ChangePage(int page)
        {
            Powerpoint.Song.ChangePage(currentPPTName, page);
        }

        void hideText()
        {
            Powerpoint.Song.HideText(currentPPTName);
        }

        void ShowText()
        {
            Powerpoint.Song.ShowText(currentPPTName);
        }

        void offDisplay()
        {
            Powerpoint.Song.SlideShowHide(currentPPTName);
            //Powerpoint.Song.OffDisplay(currentPPTName);
        }

        void onDisplay()
        {
            Powerpoint.Song.SlideShowRun(currentPPTName);
            //Powerpoint.Song.OnDisplay(currentPPTName);
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
