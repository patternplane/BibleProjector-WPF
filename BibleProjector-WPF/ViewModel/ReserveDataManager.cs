using BibleProjector_WPF.module;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class ReserveDataManager
    {
        ObservableCollection<ReserveCollectionUnit> reserveList;
        public ObservableCollection<ReserveCollectionUnit> ReserveList
        {
            get { return reserveList; }
            private set { reserveList = value; }
        }

        public ReserveDataManager()
        {
            reserveList = new ObservableCollection<ReserveCollectionUnit>();
        }

        public void addReserve(module.ReserveDataUnit reserveData)
        {
            reserveList.Add(new ReserveCollectionUnit(reserveData));
        }

        public void moveSelectionUp()
        {
            for(int i = 0; i < reserveList.Count; i++)
                if (reserveList[i].isSelected) 
                {
                    if (i == 0)
                        break;

                    reserveList.Insert(i - 1, reserveList[i]);
                    reserveList.RemoveAt(i+1);
                }
        }

        public void moveSelectionDown()
        {
            for (int i = reserveList.Count-1; i >= 0; i--)
                if (reserveList[i].isSelected)
                {
                    if (i == reserveList.Count-1)
                        break;

                    reserveList.Insert(i + 2, reserveList[i]);
                    reserveList.RemoveAt(i);
                }
        }

        public void deleteSelection()
        {
            for (int i = reserveList.Count - 1; i >= 0; i--)
                if (reserveList[i].isSelected)
                    reserveList.RemoveAt(i);
        }

        /// <summary>
        /// 리스트에서 선택된 항목들의 예약데이터 타입을 반환하며, 여러개일 경우 Null 타입으로 반환합니다.
        /// </summary>
        /// <returns></returns>
        public ReserveType getTypeOfSelection()
        {
            ReserveType type = ReserveType.NULL;
            bool finded = false;
            for (int i = 0; i < reserveList.Count; i++)
                if (reserveList[i].isSelected)
                {
                    if (finded)
                        return ReserveType.NULL;

                    type = reserveList[i].reserveType;
                    finded = true;
                }

            return type;
        }
    }

    public class ReserveCollectionUnit : INotifyPropertyChanged
    {
        public String DisplayInfo
        {
            get { return completedDisplayInfo(); }
            set { }
        }
        string completedDisplayInfo()
        {
            return "(" + reserveTypeToString(reserveData.reserveType) + ") " + reserveData.getContentInfo();
        }
        public void refreshDisplayInfo()
        {
            OnPropertyChanged("DisplayInfo");
        }

        public ReserveType reserveType
        {
            get { return reserveData.reserveType; }
            set { }
        }

        bool _isSelected = false;
        public bool isSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public module.ReserveDataUnit reserveData = null;

        public ReserveCollectionUnit(module.ReserveDataUnit reserveDataUnit)
        {
            this.reserveData = reserveDataUnit;
        }

        string reserveTypeToString(ReserveType type)
        {
            switch (type)
            {
                case ReserveType.NULL:
                    return "빈 데이터";
                case ReserveType.Bible:
                    return "성경";
                case ReserveType.Reading:
                    return "교독문";
                case ReserveType.Song:
                    return "찬양";
                case ReserveType.ExternPPT:
                    return "PPT";
                default :
                    return "오류";
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
