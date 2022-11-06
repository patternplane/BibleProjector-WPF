using BibleProjector_WPF.module;
using BibleProjector_WPF.ReserveOptionViews;
using BibleProjector_WPF.ViewModel.ReserveOptionViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    internal class ReserveManagerViewModel : INotifyPropertyChanged
    {
        module.ReserveData M_ReserveData;

        public ReserveManagerViewModel()
        {
            M_ReserveData = new module.ReserveData();
            M_ReserveData.applyPropertyChanged(reserveListUpdater);
            reserveList = ReserveCollection.makeReserveCollection(M_ReserveData);

            // 리스트 테스트용 항목들
            M_ReserveData.addReserve(new module.ReserveDataUnit());
            M_ReserveData.addReserve(new module.ReserveDataUnit());
            M_ReserveData.addReserve(new module.ReserveDataUnit());
            M_ReserveData.addReserve(new module.ReserveDataUnit());

            // 화면전환 테스트 
            rvm_bible = new ReserveOptionViewModels.Bible();
            rvm_song = new ReserveOptionViewModels.Song();
            rvm_extern = new ReserveOptionViewModels.ExternPPT();
            rvm_reading = new ReserveOptionViewModels.Reading();
            ReserveOptionViewModel = rvm_bible;
        }

        // 화면전환 테스트용
        ReserveOptionViewModels.Bible rvm_bible;
        ReserveOptionViewModels.Reading rvm_reading;
        ReserveOptionViewModels.Song rvm_song;
        ReserveOptionViewModels.ExternPPT rvm_extern;

        Collection<ReserveCollectionUnit> reserveList;
        public Collection<ReserveCollectionUnit> ReserveList
        {
            get { return reserveList; }
            set { reserveList = value; }
        }
        void reserveListUpdater(object sender, PropertyChangedEventArgs e)
        {
            reserveList = ReserveCollection.makeReserveCollection(M_ReserveData);
        }

        object reserveOptionViewModel;
        public object ReserveOptionViewModel
        {
            get { return reserveOptionViewModel; } 
            set { reserveOptionViewModel = value;
                OnPropertyChanged("ReserveOptionViewModel");
            }
        }
        
        object selectedReserveItems;
        public object SelectedReserveItems
        {
            get { return selectedReserveItems;}
            set { selectedReserveItems = value; Console.WriteLine(selectedReserveItems.ToString()); }
        }

        public void UpButtonClick()
        {
        }

        public void DownButtonClick()
        {
        }

        public void DeleteButtonClick()
        {
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
