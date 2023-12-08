using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleReserveData : NotifyPropertyChanged
    {
        // 성경 예약 리스트
        ReserveCollectionUnit[] _BibleReserveList;
        public ReserveCollectionUnit[] BibleReserveList { get { return _BibleReserveList; } set { _BibleReserveList = value; OnPropertyChanged(nameof(BibleReserveList)); } }

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
    }
}
