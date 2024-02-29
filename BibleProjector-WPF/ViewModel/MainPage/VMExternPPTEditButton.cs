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
        public string MainTitle { get; set; }
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
        public bool HasItem
        {
            get
            {
                return (currentData != null);
            }
        }

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
            }
        }

        public VMExternPPTEditButton(ExternPPTManager pptManager)
        {
            this.pptManager = pptManager;

            CAddPPTFile = new RelayCommand(AddPPTFile, CanRunAddPPTFile);
            CEditPPTFile = new RelayCommand(obj => EditPPTFile(), obj => CanRunEditPPTFile());
            CRefreshPPTFile = new RelayCommand(obj => RefreshPPTFile(), obj => CanRefreshPPTFile());
        }

        // ========== Commands ==========

        bool CanRunAddPPTFile(object filePath)
        {
            AddPPTFileError = pptManager.CanAddPPT((string)filePath);
            return (AddPPTFileError == 0);
        }

        void AddPPTFile(object filePath)
        {
            if (HasItem)
                unlinkPPTFile();
            addPPTFile((string)filePath);
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

        // ========== Methods ==========

        void addPPTFile(string fullFilePath)
        {
            currentData = pptManager.AddPPT(fullFilePath);
            MainTitle = currentData.fileName;
            OnPropertyChanged("MainTitle");
        }

        void unlinkPPTFile()
        {
            pptManager.UnlinkPPT(currentData);
            MainTitle = "";
            OnPropertyChanged("MainTitle");
            currentData = null;
        }
    }
}
