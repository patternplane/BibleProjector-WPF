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

        // ========== Members ==========

        module.ISearcher searcher = null;
        module.ReserveDataManager reserveManager;
        module.ShowStarter showStarter;
        Event.BibleSelectionEventManager bibleSelectionEventManager;

        // ========== Gen ==========

        public VMSearchControl(module.ISearcher searcher, module.ReserveDataManager reserveManager, module.ShowStarter showStarter, Event.BibleSelectionEventManager bibleSelectionEventManager)
        {
            CSearchStart = new RelayCommand(obj => SearchStart());
            CPopupHide = new RelayCommand(obj => PopupHide());
            CLastestResultShow = new RelayCommand(obj => PopupShow());
            //CItemClick = new RelayCommand(itemClick);
            CItemSelected = new RelayCommand(obj => ItemSelected((VMSearchResult)obj));
            CStartShow = new RelayCommand((obj) => StartShow());
            CReserveThis = new RelayCommand((obj) => ReserveThis());

            this.searcher = searcher;
            this.reserveManager = reserveManager;
            this.showStarter = showStarter;
            this.bibleSelectionEventManager = bibleSelectionEventManager;
        }

        // ========== Command ==========

        /*void itemClick(object obj)
        {
            module.Data.ShowData data = (module.Data.ShowData)obj;

            Console.WriteLine("Click");
        }*/

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
    }
}
