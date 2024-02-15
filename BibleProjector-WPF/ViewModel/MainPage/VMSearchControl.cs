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
        public ICollection<VMSearchResult> SearchResultList { get; set; } = new ObservableCollection<VMSearchResult>();

        public ICommand CSearchStart { get; set; }
        public ICommand CPopupHide { get; set; }
        public ICommand CLastestResultShow { get; set; }

        // ========== Gen ==========

        public VMSearchControl()
        {
            CSearchStart = new RelayCommand(obj => SearchStart());
            CPopupHide = new RelayCommand(obj => PopupHide());
            CLastestResultShow = new RelayCommand(obj => PopupShow());
        }

        // ========== Command ==========

        void SearchStart()
        {
            if (SearchText == null
                || SearchText.Length <= 1)
                return;

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
    }
}
