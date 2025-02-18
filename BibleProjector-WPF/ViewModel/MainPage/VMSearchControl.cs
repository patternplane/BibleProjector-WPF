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
        public ICollection<VMSearchResult> SearchResultList { get; set; }
        VMSearchResult _SelectionItem;
        public VMSearchResult SelectionItem { get { return _SelectionItem; } set { _SelectionItem = value; OnPropertyChanged("SelectionItem"); } }

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

        private VMSearchResult editingData;

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
            CItemSelected = new RelayCommand(obj => ItemSelected((VMSearchResult)obj));
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

            ObservableCollection<VMSearchResult> newResults = new ObservableCollection<VMSearchResult>();

            foreach (module.SearchData data in searcher.getSearchResult(SearchText))
                newResults.Add(new VMSearchResult(data));
            
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

        void ItemSelected(VMSearchResult item)
        {
            if (item.getData().isAvailData())
            {
                this.SelectionItem = item;

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

                if (item.getData() is module.Data.BibleData)
                {
                    module.Data.BibleData data = (module.Data.BibleData)item.getData();
                    bibleSelectionEventManager.InvokeBibleSelection(data.book, data.chapter, data.verse);
                }
            }
        }

        void StartShow()
        {
            if (SelectionItem == null)
                return;

            showStarter.Show(SelectionItem.getData());
        }

        void ReserveThis()
        {
            if (SelectionItem == null)
                return;

            reserveManager.AddReserveItem(this, SelectionItem.getData());
        }

        private void OpenEditor()
        {
            editingData = SelectionItem;
            VM_Modify.setupData(SelectionItem.getData());
            displayModifyView(true);
        }

        private void updateSelectedItem()
        {
            editingData.update();
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

            // ● 추후 리팩토링 요구됨 :
            //   미리보기 화면이 검색결과 항목만 받도록 되어있어
            //   불필요하게 검색 결과로 Wrapping하는 과정이 발생하고 있음.
            ItemSelected(
                new VMSearchResult(
                    new module.Data.SongSearchData(
                        newSong,
                        "(추가됨) " + newSong.songTitle, 
                        0)
                    )
                );
        }

        private void displayAddView(bool visible)
        {
            ShowAddView = visible;
            OnPropertyChanged(nameof(ShowAddView));
        }
    }
}
