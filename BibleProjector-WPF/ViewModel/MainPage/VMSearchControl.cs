using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMSearchControl : ViewModel
    {
        // ========== Properties ==========

        public string SearchText { get; set; }
        public bool ResultPopupOpen { get; set; }
        public ICollection<VMPreviewData> SearchResultList { get; set; }
        VMPreviewData _SelectionItem;
        public VMPreviewData SelectionItem { get { return _SelectionItem; } set { _SelectionItem = value; OnPropertyChanged("SelectionItem"); } }

        public IVMPreviewData PreviewData { get; private set; }
        private VMPreviewData defaultPreviewDataFrame = new VMPreviewData(null);

        public ICommand CSearchStart { get; set; }
        public ICommand CPopupHide { get; set; }
        public ICommand CLastestResultShow { get; set; }
        public ICommand CItemSelected { get; set; }
        public ICommand CStartShow { get; set; }
        public ICommand CReserveThis { get; set; }
        public ICommand COpenEditor { get; set; }
        public ICommand COpenAdder { get; set; }

        public VMModify VM_Modify { get; private set; }
        public VMLyricAdd VM_LyricAdd { get; private set; }
        public bool CanOpenEditor { get; private set; } = false;
        public bool ShowModifyView { get; private set; } = false;
        public bool ShowAddView { get; private set; } = false;

        // ========== Members ==========

        module.ISearcher searcher = null;
        module.ReserveDataManager reserveManager;
        module.ShowStarter showStarter;
        Event.BibleSelectionEventManager bibleSelectionEventManager;

        // ========== Gen ==========

        public VMSearchControl(
            module.ISearcher searcher,
            module.ReserveDataManager reserveManager, 
            module.ShowStarter showStarter,
            Event.BibleSelectionEventManager bibleSelectionEventManager,
            module.Data.SongManager songManager)
        {
            CSearchStart = new RelayCommand(obj => SearchStart());
            CPopupHide = new RelayCommand(obj => PopupHide());
            CLastestResultShow = new RelayCommand(obj => PopupShow());
            CItemSelected = new RelayCommand(obj => ItemSelected((VMPreviewData)obj));
            CStartShow = new RelayCommand((obj) => StartShow());
            CReserveThis = new RelayCommand((obj) => ReserveThis());
            COpenEditor = new RelayCommand((obj) => OpenEditor());
            COpenAdder = new RelayCommand((obj) => OpenAdder());

            this.searcher = searcher;
            this.reserveManager = reserveManager;
            this.showStarter = showStarter;
            this.bibleSelectionEventManager = bibleSelectionEventManager;

            this.VM_Modify = new VMModify(songManager);
            VM_Modify.CloseEventHandler += (sender, e) => displayModifyView(false);
            VM_Modify.ItemModified += (sender, e) => updateSelectedItem();

            this.VM_LyricAdd = new VMLyricAdd(songManager);
            VM_LyricAdd.CloseEventHandler += (sender, e) => displayAddView(false);
            VM_LyricAdd.NewItemAddedEventHandler += (sender, newSong) => changeSelectionToNewSong(newSong);
        }

        // ========== Command ==========

        void SearchStart()
        {
            if (SearchText == null
                || SearchText.Length < 1)
            {
                PopupHide();
                return;
            }

            ObservableCollection<VMPreviewData> newResults = new ObservableCollection<VMPreviewData>();

            foreach (module.SearchData data in searcher.getSearchResult(SearchText))
                newResults.Add(new VMPreviewData(data));

            this.SearchResultList = newResults; 
            OnPropertyChanged("SearchResultList");

            if (SearchResultList.Count > 0)
                PopupShow();
        }

        void PopupShow()
        {
            ResultPopupOpen = true;
            OnPropertyChanged("ResultPopupOpen");
        }

        void PopupHide()
        {
            ResultPopupOpen = false;
            OnPropertyChanged("ResultPopupOpen");
        }

        void ItemSelected(VMPreviewData item)
        {
            if (item.getData().isAvailData())
            {
                this.SelectionItem = item;
                displayItemPreview(item);

                if (item.getData() is module.Data.BibleData)
                {
                    module.Data.BibleData data = (module.Data.BibleData)item.getData();
                    bibleSelectionEventManager.InvokeBibleSelection(data.book, data.chapter, data.verse);
                }
            }
        }

        private void displayItemPreview(module.Data.IPreviewData data, bool isAdded = false)
        {
            if (data.getData().isAvailData())
            {
                defaultPreviewDataFrame.setData(data, isAdded, false);
                displayItemPreview(defaultPreviewDataFrame);
            }
        }

        private void displayItemPreview(IVMPreviewData item)
        {
            if (item.getData().isAvailData())
            {
                PreviewData = item;
                OnPropertyChanged(nameof(PreviewData));

                if (item.Type == ShowContentType.Bible
                    || item.Type == ShowContentType.Song)
                {
                    CanOpenEditor = true;
                    OnPropertyChanged(nameof(CanOpenEditor));
                }
                else
                {
                    CanOpenEditor = false;
                    OnPropertyChanged(nameof(CanOpenEditor));
                }
            }
        }

        void StartShow()
        {
            if (PreviewData == null)
                return;

            showStarter.Show(PreviewData.getData());
        }

        void ReserveThis()
        {
            if (PreviewData == null)
                return;

            reserveManager.AddReserveItem(this, PreviewData.getData());
        }

        private void OpenEditor()
        {
            VM_Modify.setupData(PreviewData.getData());
            displayModifyView(true);
        }

        private void updateSelectedItem()
        {
            PreviewData.update();
        }

        private void displayModifyView(bool visible)
        {
            ShowModifyView = visible;
            OnPropertyChanged(nameof(ShowModifyView));
        }

        private void OpenAdder()
        {
            displayAddView(true);
        }

        private void changeSelectionToNewSong(module.Data.SongData newSong)
        {
            if (newSong == null)
                return;

            displayItemPreview(newSong, true);
        }

        private void displayAddView(bool visible)
        {
            ShowAddView = visible;
            OnPropertyChanged(nameof(ShowAddView));
        }
    }
}
