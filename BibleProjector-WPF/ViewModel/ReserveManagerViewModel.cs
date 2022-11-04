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
    // 예약항목 데이터 한 단위
    public class ReserveDataUnit
    {
        string displayInfo;
        public String DisplayInfo
        {
            get { return displayInfo; }
            set { displayInfo = value; }
        }
    }

    internal class ReserveManagerViewModel : INotifyPropertyChanged
    {
        public ReserveManagerViewModel()
        {
            reserveList = new ObservableCollection<ReserveDataUnit>();

            // 리스트 테스트용 항목들
            reserveList.Add(new ReserveDataUnit() { DisplayInfo="dsaddddddddddddddfdsafsdfadsffdsaafs"});
            reserveList.Add(new ReserveDataUnit());
            reserveList.Add(new ReserveDataUnit());
            reserveList.Add(new ReserveDataUnit());

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

        ObservableCollection<ReserveDataUnit> reserveList;
        public ObservableCollection<ReserveDataUnit> ReserveList
        {
            get { return reserveList; }
            set { reserveList = value; }
        }

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
