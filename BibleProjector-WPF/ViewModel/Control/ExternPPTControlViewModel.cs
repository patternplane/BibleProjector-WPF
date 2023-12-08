using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BibleProjector_WPF.ViewModel
{
    class ExternPPTControlViewModel : NotifyPropertyChanged
    {

        // ================================================ 세팅 ================================================

        public void ShowExternPPT(string fileName, int StartSlide)
        {
            currentFileName = fileName;
            newExternPPTSetting(fileName, StartSlide);
        }

        void SetImages(string fileName)
        {
            PPTImages.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Powerpoint.ExternPPTs.getThumbnailPathWithGenerator(fileName));

            List<SingleSlidePreview> slideData = new List<SingleSlidePreview>(50);
            BitmapImage bi;
            foreach (System.IO.FileInfo f in di.GetFiles())
            {
                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(f.FullName, UriKind.Absolute);
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                slideData.Add(new SingleSlidePreview() { Number = int.Parse(module.StringModifier.makeOnlyNum(f.Name)), Image = bi });
            }

            slideData.Sort((a, b) => (a.Number.CompareTo(b.Number)));
            PPTImages = new BindingList<SingleSlidePreview>(slideData);
        }

        public void RefreshExternPPT(string fileName)
        {
            if (fileName.CompareTo(currentFileName) != 0)
                return;

            int temp = CurrentPageIndex;

            SetImages(fileName);

            CurrentPageIndex = temp;
        }

        public void HideExternPPT()
        {
            SlideShowHide();
        }

        private void newExternPPTSetting(string fileName,int StartSlide)
        {
            setCurrentPPTInfo(fileName);

            SetImages(fileName);

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
                OnPropertyChanged();
            } 
        }

        // 현재 ppt 이름 표시 - Text
        // public string CurrentPPTInfo { get; set; }
        public string WindowTitle { get; set; } = "";

        // ppt 페이지 리스트박스
        private BindingList<SingleSlidePreview> PPTImages_in = new BindingList<SingleSlidePreview>();
        public BindingList<SingleSlidePreview> PPTImages { get { return PPTImages_in; } set { PPTImages_in = value; OnPropertyChanged(); } }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex { get { return CurrentPageIndex_in; } set { CurrentPageIndex_in = value;
                if (CurrentPageIndex_in != -1)
                {
                    // 페이지 변경 처리
                    CurrentPageIndex_in = goToSlide(CurrentPageIndex_in + 1) - 1;
                }
                OnPropertyChanged();
            } }

        public class SingleSlidePreview
        {
            public int Number { get; set; }
            public BitmapImage Image {get; set;}
        }

        // ================================================ 속성 메소드 ================================================

        void setCurrentPPTInfo(string fileName)
        {
            WindowTitle = "PPT(" + fileName + ")" ;
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

        int inputedNum = 0;
        public void NumInput(int num)
        {
            inputedNum = inputedNum * 10 + num;
            if (inputedNum < 0)
                inputedNum = int.MaxValue;
        }

        public void NumInput_Enter()
        {
            if (inputedNum > 0)
            {
                if (inputedNum > PPTImages.Count)
                    CurrentPageIndex = PPTImages.Count - 1;
                else
                    CurrentPageIndex = inputedNum - 1;

                inputedNum = 0;
            }
        }

        public void NumInput_Remove()
        {
            inputedNum = 0;
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
    }
}
