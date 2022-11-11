using BibleProjector_WPF.module;
using BibleProjector_WPF.ReserveOptionViews;
using BibleProjector_WPF.ViewModel.ReserveOptionViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel
{
    internal class ReserveManagerViewModel : INotifyPropertyChanged
    {
        public ReserveManagerViewModel()
        {
            reserveDataManager = new ReserveDataManager();
            
            // 옵션 탭
            ROViewModels[(int)ROViewModel.Null] = new ReserveOptionViewModels.Null();
            ROViewModels[(int)ROViewModel.Bible] = new ReserveOptionViewModels.Bible();
            ROViewModels[(int)ROViewModel.Reading] = new ReserveOptionViewModels.Reading();
            ROViewModels[(int)ROViewModel.Song] = new ReserveOptionViewModels.Song();
            ROViewModels[(int)ROViewModel.ExternPPT] = new ReserveOptionViewModels.ExternPPT();
            ReserveOptionViewModel = ROViewModels[(int)ROViewModel.Null];

            // 리스트 테스트용 항목들
            reserveDataManager.addReserve(new module.EmptyReserveDataUnit());
            reserveDataManager.addReserve(new module.BibleReserveDataUnit("05","003","005"));
            reserveDataManager.addReserve(new module.BibleReserveDataUnit("15", "006", "010"));
            reserveDataManager.addReserve(new module.BibleReserveDataUnit("1", "007", "015"));
        }

        ReserveDataManager reserveDataManager;
        public ReserveDataManager ReserveDataManager
        {
            get { return reserveDataManager; }
            private set { reserveDataManager = value; }
        }

        enum ROViewModel
        {
            Null,
            Bible,
            Reading,
            Song,
            ExternPPT
        }
        object[] ROViewModels = new object[5];
        object reserveOptionViewModel;
        public object ReserveOptionViewModel
        {
            get { return reserveOptionViewModel; } 
            set { reserveOptionViewModel = value;
                OnPropertyChanged("ReserveOptionViewModel");
            }
        }

        public void UpButtonClick()
        {
            reserveDataManager.moveSelectionUp();
        }

        public void DownButtonClick()
        {
            reserveDataManager.moveSelectionDown();
        }

        public void DeleteButtonClick()
        {
            reserveDataManager.deleteSelection();
        }

        public void ListKeyInputed(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                reserveDataManager.deleteSelection();
        }

        public void ListSelectionChanged()
        {
            switch (ReserveDataManager.getTypeOfSelection())
            {
                case ReserveType.Bible:
                    ReserveOptionViewModel = ROViewModels[(int)ROViewModel.Bible];
                    break;
                case ReserveType.Reading:
                    ReserveOptionViewModel = ROViewModels[(int)ROViewModel.Reading];
                    break;
                case ReserveType.Song_CCM:
                case ReserveType.Song_Hymn:
                    ReserveOptionViewModel = ROViewModels[(int)ROViewModel.Song];
                    break;
                case ReserveType.ExternPPT:
                    ReserveOptionViewModel = ROViewModels[(int)ROViewModel.ExternPPT];
                    break;
                default :
                    ReserveOptionViewModel = ROViewModels[(int)ROViewModel.Null];
                    break;
            }
        }

        // INotifyPropertyChanged 인터페이스 관련

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
