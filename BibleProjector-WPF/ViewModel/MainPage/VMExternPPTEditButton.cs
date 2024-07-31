using BibleProjector_WPF.module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMExternPPTEditButton : ViewModel
    {
        public string FilePath
        {
            get
            {
                return currentData?.fileFullPath;
            }
        }
        public ICommand CAddPPTFile { get; set; }
        public int AddPPTFileError { get; private set; }
        public int MaxSlideSize
        {
            get
            {
                return ExternPPTManager.MAX_SLIDE_COUNT;
            }
        }
        public ICommand CEditPPTFile { get; set; }
        public ICommand CRefreshPPTFile { get; set; }
        public int RefreshPPTFileError { get; private set; }
        public ICommand CDeletePPTFile { get; set; }
        public ICommand CShowRun { get; set; }
        public ICommand CDoReserve { get; set; }
        public bool OnShift { get; private set; }

        void OnKeyInputTask(Key key, bool isDown)
        {
            if (key == Key.LeftShift)
            {
                OnShift = isDown;
                OnPropertyChanged("OnShift");
            }
        }

        ReserveDataManager reserveManager;
        ExternPPTManager pptManager;
        module.Data.ExternPPTData _currentData = null;
        module.Data.ExternPPTData currentData
        {
            get
            {
                return _currentData;
            }
            set
            {
                _currentData = value;
                OnPropertyChanged("HasItem");
                OnPropertyChanged("MainTitle");
            }
        }
        public bool HasItem { get { return (currentData != null); } }
        public string MainTitle { get { return currentData?.fileName; } }
        ShowStarter showStarter;

        int myIdx;

        public VMExternPPTEditButton(ReserveDataManager reserveManager, ExternPPTManager pptManager, Event.KeyInputEventManager keyInputEventManager, int idx, ShowStarter showStarter)
        {
            this.reserveManager = reserveManager;
            this.pptManager = pptManager;
            keyInputEventManager.KeyDown += OnKeyInputTask;
            this.showStarter = showStarter;
            this.myIdx = idx;

            CAddPPTFile = new RelayCommand(AddPPTFile, CanRunAddPPTFile);
            CEditPPTFile = new RelayCommand(obj => EditPPTFile(), obj => CanRunEditPPTFile());
            CRefreshPPTFile = new RelayCommand(obj => RefreshPPTFile(), obj => CanRefreshPPTFile());
            CDeletePPTFile = new RelayCommand(obj => DeletePPTFile(), obj => { return HasItem; });
            CShowRun = new RelayCommand(obj => ShowRun());
            CDoReserve = new RelayCommand(obj => DoReserve());

            currentData = pptManager.getMyData(myIdx);
        }

        // ========== Commands ==========

        bool CanRunAddPPTFile(object filePath)
        {
            AddPPTFileError = pptManager.CanAddPPT((string)filePath);
            return (AddPPTFileError == 0);
        }

        void ShowRun()
        {
            if (HasItem)
                showStarter.Show(currentData);
        }

        void AddPPTFile(object filePath)
        {
            if (HasItem)
                unlinkPPTFile();
            addPPTFile((string)filePath);
        }

        void DeletePPTFile()
        {
            unlinkPPTFile();
        }

        bool CanRunEditPPTFile()
        {
            return currentData.isOriginFileExist();
        }

        void EditPPTFile()
        {
            currentData.ModifyOpenPPT();
        }

        bool CanRefreshPPTFile()
        {
            RefreshPPTFileError = pptManager.CanRefreshPPT(currentData.fileFullPath);
            return (RefreshPPTFileError == 0);
        }

        void RefreshPPTFile() {
            currentData.RefreshPPT();
        }

        void DoReserve()
        {
            if (HasItem)
                reserveManager.AddReserveItem(this, currentData);
        }

        // ========== Methods ==========

        void addPPTFile(string fullFilePath)
        {
            currentData = pptManager.AddPPT(fullFilePath, myIdx);
        }

        void unlinkPPTFile()
        {
            pptManager.UnlinkPPT(myIdx);
            currentData = null;
        }
    }
}
