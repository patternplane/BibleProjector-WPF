using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMShowControler : ViewModel
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public ShowContentType ContentType { get; set; }

        // ================================================ 속성 ================================================

        public ICommand CDisplayOnOff { get; set; }
        public ICommand CTextShowHide { get; set; }
        public ICommand CGoNextPage { get; set; }
        public ICommand CGoPreviousPage { get; set; }
        public ICommand CSetDisplayTopMost { get; set; }

        // 성경 페이지 리스트박스
        public ObservableCollection<string> BiblePages { get; set; }
        private int CurrentPageIndex_in;
        public int CurrentPageIndex
        {
            get { return CurrentPageIndex_in; }
            set
            {
                if (CurrentPageIndex_in != value)
                {
                    CurrentPageIndex_in = value;
                    if (CurrentPageIndex_in != -1)
                        // 페이지 변경 처리
                        Powerpoint.Bible.Change_VerseContent(verse.ToString(), BiblePages[CurrentPageIndex_in], CurrentPageIndex_in == 0);
                }
            }
        }

        // 이전 페이지 넘기는 동작 설정
        public bool preview_GoLastPage { get; set; } = true;

        string Kjjeul_in;
        string Kjjeul
        {
            get { return Kjjeul_in; }
            set
            {
                Kjjeul_in = value;
                bible_display = Database.getTitle(Kjjeul_in.Substring(0, 2));
                chapter = int.Parse(Kjjeul_in.Substring(2, 3));
                verse = int.Parse(Kjjeul_in.Substring(5, 3));
            }
        }
        string bible_display = null;
        int chapter = -1;
        int verse = -1;

        // ================================================ 세팅 ================================================

        public VMShowControler()
        {
            CDisplayOnOff = new RelayCommand(obj => DisplayVisibility((bool)obj));
            CTextShowHide = new RelayCommand(obj => TextVisibility((bool)obj));
            CGoNextPage = new RelayCommand(obj => GoNextPage());
            CGoPreviousPage = new RelayCommand(obj => GoPreviousPage());
            CSetDisplayTopMost = new RelayCommand(obj => SetDisplayTopMost());
        }   

        private void BiblePresentationSetting(string Kjjeul)
        {
            this.Kjjeul = Kjjeul;

            Title1 = bible_display;
            Title2 = chapter + "장 " + verse + "절";
            OnPropertyChanged("Title1");
            OnPropertyChanged("Title2");

            BiblePages = new ObservableCollection<string>(
                module.StringModifier.makeStringPage(
                    Database.getBible(Kjjeul)
                    , module.ProgramOption.Bible_CharPerLine
                    , module.ProgramOption.Bible_LinePerSlide
                    )
                );
            OnPropertyChanged("BiblePages");

            Powerpoint.Bible.Change_BibleChapter(bible_display, chapter.ToString());
        }

        // ================================================ 이벤트에 쓰일 함수 ================================================

        public void DisplayVisibility(bool OnDisplay)
        {
            if (OnDisplay)
                Powerpoint.Bible.SlideShowRun();
            else
                Powerpoint.Bible.SlideShowHide();
        }

        public void TextVisibility(bool ShowText)
        {
            if (ShowText)
                Powerpoint.Bible.ShowText();
            else
                Powerpoint.Bible.HideText();
        }

        public void SetDisplayTopMost()
        {
            Powerpoint.Bible.TopMost();
        }

        void MovePage(int pageIdx)
        {
            CurrentPageIndex = pageIdx;
            OnPropertyChanged("CurrentPageIndex");
        }

        public void showBible(string Kjjeul)
        {
            BiblePresentationSetting(Kjjeul);

            MovePage(0);
            OnPropertyChanged("CurrentPageIndex");
            TextVisibility(true);
            DisplayVisibility(true);
        }

        public void GoNextPage()
        {
            if (CurrentPageIndex != BiblePages.Count - 1)
                MovePage(CurrentPageIndex + 1);
            else
            {
                BiblePresentationSetting(Database.getBibleIndex_Next(Kjjeul));
                MovePage(0);

                // Bible.tempBibleAccesser.applyBibleMoving(Kjjeul.Substring(0, 2), Kjjeul.Substring(2, 3), Kjjeul.Substring(5, 3));
            }
        }

        public void GoPreviousPage()
        {
            if (CurrentPageIndex != 0)
                MovePage(CurrentPageIndex - 1);
            else
            {
                BiblePresentationSetting(Database.getBibleIndex_Previous(Kjjeul));
                if (preview_GoLastPage)
                    MovePage(BiblePages.Count - 1);
                else
                    MovePage(0);

                // Bible.tempBibleAccesser.applyBibleMoving(Kjjeul.Substring(0, 2), Kjjeul.Substring(2, 3), Kjjeul.Substring(5, 3));
            }
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
                if (inputedNum > BiblePages.Count)
                    MovePage(BiblePages.Count - 1);
                else
                    MovePage(inputedNum - 1);

                inputedNum = 0;
            }
        }

        public void NumInput_Remove()
        {
            inputedNum = 0;
        }
    }
}
