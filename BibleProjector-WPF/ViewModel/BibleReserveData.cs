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
            module.ProgramData.getBibleReserveData(this);

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

        // ======================== ?? ========================

        // 데이터 저장할 때 출력 규격
        public string getSaveData()
        {
            StringBuilder str = new StringBuilder(50).Clear();
            foreach (ReserveCollectionUnit item in BibleReserveList)
            {
                /*str.Append(item.Book);
                str.Append(" ");
                str.Append(item.Chapter);
                str.Append(" ");
                str.Append(item.Verse);
                str.Append("\r\n");*/
            }
            return str.ToString();
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
