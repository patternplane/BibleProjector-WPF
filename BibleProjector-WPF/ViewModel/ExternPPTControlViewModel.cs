using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BibleProjector_WPF.ViewModel
{
    class ExternPPTControlViewModel : INotifyPropertyChanged
    {

        // ================================================ 세팅 ================================================

        public ExternPPTControlViewModel(string fileName, int StartSlide)
        {
            ShowExternPPT(fileName,StartSlide);
        }

        public void ShowExternPPT(string fileName, int StartSlide)
        {
            currentFileName = fileName;
            newExternPPTSetting(fileName, StartSlide);
        }

        public void RefreshExternPPT(string fileName)
        {
            if (fileName.CompareTo(currentFileName) != 0)
                return;

            int temp = CurrentPageIndex;

            BitmapImage bi;
            PPTImages.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Powerpoint.ExternPPTs.getThumbnailPath(currentFileName));
            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(f.FullName, UriKind.Absolute);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                PPTImages.Add(bi);
            }
            CurrentPageIndex = temp;
        }

        public void HideExternPPT()
        {
            SlideShowHide();
        }

        private void newExternPPTSetting(string fileName,int StartSlide)
        {
            setCurrentPPTInfo(fileName);

            BitmapImage bi;
            PPTImages.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Powerpoint.ExternPPTs.getThumbnailPath(fileName));
            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(f.FullName, UriKind.Absolute);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                PPTImages.Add(bi);
            }
            CurrentPageIndex = StartSlide - 1;

            isDisplayShow = true;
        }

        // 현재 사용중인 ppt 이름
        string currentFileName { get; set; }
        public bool isSameFileName(string FileName)
        {
            return (currentFileName.CompareTo(FileName) == 0);
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

        // 현재 ppt 이름 표시 - Text
        public string CurrentPPTInfo { get; set; }

        // ppt 페이지 리스트박스
        private BindingList<BitmapImage> PPTImages_in = new BindingList<BitmapImage>();
        public BindingList<BitmapImage> PPTImages { get { return PPTImages_in; } set { PPTImages_in = value; NotifyPropertyChanged(); } }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                {
                    // 페이지 변경 처리
                    CurrentPageIndex_in = goToSlide(CurrentPageIndex_in + 1) - 1;
                }
                NotifyPropertyChanged();
            } }

        // ================================================ 속성 메소드 ================================================

        void setCurrentPPTInfo(string fileName)
        {
            CurrentPPTInfo = fileName;
        }

        // ================================================ 이벤트에 쓰일 함수 ================================================

        public void RunNextPage()
        {
            if (CurrentPageIndex != PPTImages.Count - 1)
                CurrentPageIndex++;
        }

        public void RunPreviousPage()
        {
            if (CurrentPageIndex != 0)
                CurrentPageIndex--;
        }

        public void RunTopMost()
        {
            Powerpoint.ExternPPTs.TopMost(currentFileName);
        }

        // ================================================ 동작 ================================================

        void SlideShowRun()
        {
            Powerpoint.ExternPPTs.SlideShowRun(currentFileName);
        }

        int goToSlide(int slideIndex)
        {
            return Powerpoint.ExternPPTs.goToSlide(currentFileName, slideIndex);
        }

        void SlideShowHide()
        {
            Powerpoint.ExternPPTs.SlideShowHide(currentFileName);
        }

        void offDisplay()
        {
            Powerpoint.ExternPPTs.SlideShowHide(currentFileName);
            //Powerpoint.Reading.OffDisplay();
        }

        void onDisplay()
        {
            Powerpoint.ExternPPTs.SlideShowRun(currentFileName);
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
