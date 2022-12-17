using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleReserveData : INotifyPropertyChanged
    {
        // 성경 예약 리스트
        ReserveCollectionUnit[] _BibleReserveList;
        public ReserveCollectionUnit[] BibleReserveList { get { return _BibleReserveList; } set { _BibleReserveList = value; NotifyPropertyChanged(nameof(BibleReserveList)); } }

        public BibleReserveData()
        {
            ReserveDataManager.subscriptToListChange(ListUpdate);
        }

        // ======================== 리스트 데이터 참조 ========================

        ReserveCollectionUnit[] getBibleReserveList()
        {
            return ReserveDataManager.instance.ReserveList.Where
                (obj => (obj.reserveType == module.ReserveType.Bible)).ToArray();
        }

        void ListUpdate(object sender, Event.ReserveListChangedEventArgs e) 
        {
            if ((e.changeType & Event.ReserveUpdateType.Bible) > 0)
                BibleReserveList = getBibleReserveList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
