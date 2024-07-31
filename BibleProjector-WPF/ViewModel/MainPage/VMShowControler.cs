using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMShowControler : ViewModel
    {

        // ================================================ Binding Properties ================================================

        public ICommand CDisplayOnOff { get; }
        public ICommand CTextShowHide { get; }
        public ICommand CGoNextPage { get; }
        public ICommand CGoPreviousPage { get; }
        public ICommand CSetDisplayTopMost { get; }

        public ShowContentType ContentType { get; set; }

        private Event.BibleSelectionEventManager bibleSelectionEventManager;

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

        public bool DisplayButtonState { get; set; } = false;
        public bool TextButtonState { get; set; } = false;

        private bool _hasFocus;
        public bool hasFocus
        {
            get { return _hasFocus; }
            set { _hasFocus = value; OnPropertyChanged(nameof(isActive)); }
        }
        private bool activation;
        public bool isActive { get { return hasFocus && activation; } }

        public bool doFastPass { get; private set; } = false;

        // 애니메이션 효과
        public bool DoAnimation { get; set; } = false;

        // ================================================ Properties ================================================

        module.Data.ShowData currentData;

        // ================================================ 세팅 ================================================

        public VMShowControler(ShowContentType type, module.ShowStarter showStarter,Event.WindowActivateChangedEventManager windowActivateChangedEventManager , Event.BibleSelectionEventManager bibleSelectionEventManager = null)
        {
            CDisplayOnOff = new RelayCommand(obj => DisplayVisibility((bool)obj));
            CTextShowHide = new RelayCommand(obj => TextVisibility((bool)obj));
            CGoNextPage = new RelayCommand(obj => GoNextPage((bool)obj || doFastPass));
            CGoPreviousPage = new RelayCommand(obj => GoPreviousPage((bool)obj || doFastPass));
            CSetDisplayTopMost = new RelayCommand(obj => SetDisplayTopMost());

            this.ContentType = type;
            this.bibleSelectionEventManager = bibleSelectionEventManager;
            if (type == ShowContentType.Bible && bibleSelectionEventManager == null)
                throw new Exception("성경 컨트롤러는 BibleSelectionEventManager 객체를 요구합니다. (부족한 생성자 매개변수)");

            windowActivateChangedEventManager.windowActivateChanged += UIActiveChanged;

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

        public void doViewModelChanged()
        {
            DoAnimation = true;
            OnPropertyChanged("DoAnimation");
            DoAnimation = false;
            OnPropertyChanged("DoAnimation");

            hasFocus = true;
            OnPropertyChanged("hasFocus");
        }

        private void UIActiveChanged(bool isActive)
        {
            if (activation != isActive)
            {
                activation = isActive;
                OnPropertyChanged(nameof(isActive));
            }
        }

        public void keyInputed(Key inputKey, bool isDown)
        {
            if (inputKey == Key.LeftCtrl
                || inputKey == Key.RightCtrl)
            {
                if (isDown)
                {
                    doFastPass = true;
                    OnPropertyChanged(nameof(doFastPass));
                }
                else
                {
                    doFastPass = false;
                    OnPropertyChanged(nameof(doFastPass));
                }
            }

            if (hasFocus && isDown)
            {
                switch (inputKey)
                {
                    case Key.Enter:
                    case Key.Up:
                    case Key.Right:
                        GoNextPage(doFastPass);
                        break;
                    case Key.Back:
                    case Key.Down:
                    case Key.Left:
                        GoPreviousPage(doFastPass);
                        break;
                }
            }

            // 언제 숫자입력값을 다시 초기화해야 할지 곰곰히 생각해봐야 할 듯 합니다.
            /*void Window_Activated(object sender, EventArgs e)
            {
                VM_BibleControl.NumInput_Remove();
            }*//*

            void Window_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9
                    || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    VM_BibleControl.NumInput(KeyToNum(e.Key));
                    return;
                }

                switch (e.Key)
                {
                    case Key.Up:
                    case Key.Right:
                        VM_BibleControl.RunNextPage();
                        break;
                    case Key.Down:
                    case Key.Left:
                        VM_BibleControl.RunPreviousPage();
                        break;
                    case Key.Enter:
                        VM_BibleControl.NumInput_Enter();
                        break;
                }

                VM_BibleControl.NumInput_Remove();
            }
            int KeyToNum(Key key)
            {
                if (key >= Key.D0 && key <= Key.D9)
                    return (key - Key.D0);
                else if (key >= Key.NumPad0 && key <= Key.NumPad9)
                    return (key - Key.NumPad0);
                else
                    return -1;
            }*/
        }

        public void newShowStart(module.Data.ShowData data)
        {
            data.ItemRefreshedEvent += refreshData;
            data.ItemDeletedEvent += itemDeleted;

            dataSetter(data);
            MovePage(0);
            TextVisibility(true);
            DisplayVisibility(true);
            SetDisplayTopMost();

            hasFocus = true;
            OnPropertyChanged("hasFocus");
        }

        public void refreshData(object sender, EventArgs e)
        {
            dataSetter((module.Data.ShowData)sender);
            if (CurrentPageIndex < 0)
                MovePage(0);
            else if (CurrentPageIndex >= Pages.Count)
                MovePage(Pages.Count - 1);
            else
                MovePage(CurrentPageIndex);
        }

        public void itemDeleted(object sender, EventArgs e)
        {
            if (this.currentData.isSameData((module.Data.ShowData)sender))
            {
                DisplayVisibility(false);
                dataSetter(null);
            }
        }

        void dataSetter(module.Data.ShowData data)
        {
            if (ContentType == ShowContentType.Bible)
                bibleSelectionEventManager.InvokeBibleSelection(((module.Data.BibleData)data).book, ((module.Data.BibleData)data).chapter, ((module.Data.BibleData)data).verse);

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
            }
        }

        void MovePage(int pageIdx)
        {
            if (pageIdx >= -1 && pageIdx < Pages.Count)
            {
                _CurrentPageIndex = pageIdx;
                OnPropertyChanged("CurrentPageIndex");
                if (pageIdx != -1)
                    Powerpoint.setPageData(currentData, pageIdx);
            }
        }

        // ================================================ 커맨드에 쓰일 함수 ================================================

        public void DisplayVisibility(bool OnDisplay)
        {
            if (currentData == null)
                return;

            if (OnDisplay)
                Powerpoint.SlideShowRun(currentData);
            else
                Powerpoint.SlideShowHide(currentData);
            DisplayButtonState = !OnDisplay;
            OnPropertyChanged("DisplayButtonState");
        }

        public void TextVisibility(bool ShowText)
        {
            if (currentData == null)
                return;

            if (ShowText)
                Powerpoint.ShowText(currentData);
            else
                Powerpoint.HideText(currentData);
            TextButtonState = !ShowText;
            OnPropertyChanged("TextButtonState");
        }

        public void SetDisplayTopMost()
        {
            if (currentData == null)
                return;

            Powerpoint.TopMost(currentData);
        }

        public void GoNextPage(bool fastPass)
        {
            if (currentData == null)
                return;

            if (fastPass || CurrentPageIndex == Pages.Count - 1)
            {
                module.Data.ShowData nextData = currentData.getNextShowData();
                if (nextData != null)
                {
                    dataSetter(nextData);
                    MovePage(0);

                    // Bible.tempBibleAccesser.applyBibleMoving(Kjjeul.Substring(0, 2), Kjjeul.Substring(2, 3), Kjjeul.Substring(5, 3));
                }
                else if (CurrentPageIndex < Pages.Count - 1)
                    MovePage(CurrentPageIndex + 1);
            }
            else
                MovePage(CurrentPageIndex + 1);
        }

        public void GoPreviousPage(bool fastPass)
        {
            if (currentData == null)
                return;

            if (fastPass || CurrentPageIndex == 0)
            {
                module.Data.ShowData prevData = currentData.getPrevShowData();
                if (prevData != null)
                {
                    dataSetter(prevData);
                    if (fastPass)
                        MovePage(0);
                    else
                        MovePage(Pages.Count - 1);

                    // Bible.tempBibleAccesser.applyBibleMoving(Kjjeul.Substring(0, 2), Kjjeul.Substring(2, 3), Kjjeul.Substring(5, 3));
                }
                else if (CurrentPageIndex > 0)
                    MovePage(CurrentPageIndex - 1);
            }
            else
                MovePage(CurrentPageIndex - 1);
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
