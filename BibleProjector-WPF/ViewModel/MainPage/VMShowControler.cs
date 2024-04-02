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

        // ================================================ Binding Properties ================================================

        public ICommand CDisplayOnOff { get; set; }
        public ICommand CTextShowHide { get; set; }
        public ICommand CGoNextPage { get; set; }
        public ICommand CGoPreviousPage { get; set; }
        public ICommand CSetDisplayTopMost { get; set; }

        public ShowContentType ContentType { get; set; }

        public string Title1 { get; set; }
        public string Title2 { get; set; }

        public ObservableCollection<VMShowItem> Pages { get; set; }
        int _CurrentPageIndex;
        public int CurrentPageIndex 
        { 
            get 
            { 
                return _CurrentPageIndex; 
            } 
            set 
            { 
                if (_CurrentPageIndex != value) 
                    MovePage(value);  
            } 
        }

        // 이전 페이지로 넘어갈 때 동작 설정
        public bool preview_GoLastPage { get; set; } = true;

        // 애니메이션 효과
        public bool DoAnimation { get; set; } = false;

        // ================================================ Properties ================================================

        module.Data.ShowData currentData;

        // ================================================ 세팅 ================================================

        public VMShowControler(ShowContentType type, module.ShowStarter showStarter)
        {
            CDisplayOnOff = new RelayCommand(obj => DisplayVisibility((bool)obj));
            CTextShowHide = new RelayCommand(obj => TextVisibility((bool)obj));
            CGoNextPage = new RelayCommand(obj => GoNextPage());
            CGoPreviousPage = new RelayCommand(obj => GoPreviousPage());
            CSetDisplayTopMost = new RelayCommand(obj => SetDisplayTopMost());

            this.ContentType = type;

            module.ProgramOption.FrameDeletedEvent += FrameDeleted;
            showStarter.ShowStartEvent += startShow;
        }

        public void FrameDeleted(object sender, Event.FrameDeletedEventArgs e)
        {
            DisplayVisibility(false);
        }

        public void startShow(object sender, Event.ShowStartEventArgs e)
        {
            if (e.showData.getDataType() == ContentType)
                newShowStart(e.showData);
        }

        // ================================================ 메소드 ================================================

        public void doViewModelChangedAnimation()
        {
            DoAnimation = true;
            OnPropertyChanged("DoAnimation");
        }

        public void newShowStart(module.Data.ShowData data)
        {
            data.ItemRefreshedEvent += refreshData;
            data.ItemDeletedEvent += itemDeleted;

            dataSetter(data);
            MovePage(0);
            TextVisibility(true);
            DisplayVisibility(true);
        }

        public void refreshData(object sender, EventArgs e)
        {
            dataSetter((module.Data.ShowData)sender, CurrentPageIndex);
        }

        public void itemDeleted(object sender, EventArgs e)
        {
            if (this.currentData.isSameData((module.Data.ShowData)sender))
            {
                DisplayVisibility(false);
                dataSetter(null);
            }
        }

        void dataSetter(module.Data.ShowData data, int pageIdx = 0)
        {
            this.currentData = data;

            if (data == null)
            {
                this.Title1 = null;
                OnPropertyChanged("Title1");
                this.Title2 = null;
                OnPropertyChanged("Title2");
                this.Pages = null;
                OnPropertyChanged("Pages");
            }
            else
            {
                this.Title1 = currentData.getTitle1();
                OnPropertyChanged("Title1");
                this.Title2 = currentData.getTitle2();
                OnPropertyChanged("Title2");
                ObservableCollection<VMShowItem> displayPages = new ObservableCollection<VMShowItem>();
                int i = 1;
                foreach (module.Data.ShowContentData item in currentData.getContents())
                    displayPages.Add(new VMShowItem(i++, item.DisplayData, item.DoHighlight));
                this.Pages = displayPages;
                OnPropertyChanged("Pages");

                if (pageIdx >= Pages.Count)
                    pageIdx = Pages.Count - 1;
                Powerpoint.setPageData(currentData, pageIdx);
            }
        }

        void MovePage(int pageIdx)
        {
            if (pageIdx >= 0 && pageIdx < Pages.Count)
            {
                _CurrentPageIndex = pageIdx;
                OnPropertyChanged("CurrentPageIndex");
                Powerpoint.setPageData(currentData, pageIdx);
            }
        }

        // ================================================ 이벤트에 쓰일 함수 ================================================

        public void DisplayVisibility(bool OnDisplay)
        {
            if (currentData == null)
                return;

            if (OnDisplay)
                Powerpoint.SlideShowRun(currentData);
            else
                Powerpoint.SlideShowHide(currentData);
        }

        public void TextVisibility(bool ShowText)
        {
            if (currentData == null)
                return;

            if (ShowText)
                Powerpoint.ShowText(currentData);
            else
                Powerpoint.HideText(currentData);
        }

        public void SetDisplayTopMost()
        {
            if (currentData == null)
                return;

            Powerpoint.TopMost(currentData);
        }

        public void GoNextPage()
       {
            if (currentData == null)
                return;

            if (CurrentPageIndex != Pages.Count - 1)
                MovePage(CurrentPageIndex + 1);
            else
            {
                module.Data.ShowData nextData = currentData.getNextShowData();
                if (nextData != null)
                {
                    dataSetter(nextData);
                    MovePage(0);

                    // Bible.tempBibleAccesser.applyBibleMoving(Kjjeul.Substring(0, 2), Kjjeul.Substring(2, 3), Kjjeul.Substring(5, 3));
                }
            }
        }

        public void GoPreviousPage()
        {
            if (currentData == null)
                return;

            if (CurrentPageIndex != 0)
                MovePage(CurrentPageIndex - 1);
            else
            {
                module.Data.ShowData prevData = currentData.getPrevShowData();
                if (prevData != null)
                {
                    dataSetter(prevData);
                    if (preview_GoLastPage)
                        MovePage(Pages.Count - 1);
                    else
                        MovePage(0);

                    // Bible.tempBibleAccesser.applyBibleMoving(Kjjeul.Substring(0, 2), Kjjeul.Substring(2, 3), Kjjeul.Substring(5, 3));
                }
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
                if (inputedNum > Pages.Count)
                    MovePage(Pages.Count - 1);
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
