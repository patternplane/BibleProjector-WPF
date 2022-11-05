using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
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

    // 예약항목 데이터
    public class ReserveData : INotifyPropertyChanged
    {
        Collection<ReserveDataUnit> reserveList;
        
        public ReserveData()
        {
            reserveList = new Collection<ReserveDataUnit>();
        }

        public void applyPropertyChanged(PropertyChangedEventHandler handler)
        {
            PropertyChanged += handler;
        }

        public Collection<ReserveDataUnit> getReserveList()
        {
            return new Collection<ReserveDataUnit>(reserveList);
        }

        public void addReserve(ReserveDataUnit data)
        {
            reserveList.Add(data);
            OnPropertyChanged("reserveList");
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
