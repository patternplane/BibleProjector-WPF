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
    public enum ReserveSelectionsType
    {
        NoneSelected,
        Mixed,
        NULL_Single,
        NULL_Multi,
        Bible_Single,
        Bible_Multi,
        Reading_Single,
        Reading_Multi,
        Song_Single,
        Song_Multi,
        ExternPPT_Single,
        ExternPPT_Multi
    }

    public class ReserveDataManager
    {
        // 이거 말고 더 좋은 방법 있을텐데
        static public ReserveDataManager instance;

        ObservableCollection<ReserveCollectionUnit> reserveList;
        public ObservableCollection<ReserveCollectionUnit> ReserveList
        {
            get { return reserveList; }
            private set { reserveList = value; }
        }

        public ReserveDataManager()
        {
            instance = this;

            reserveList = new ObservableCollection<ReserveCollectionUnit>();
        }

        public bool ExternPPT_isNotOverlaped(string filePath)
        {
            foreach (ReserveCollectionUnit item in ReserveList)
                if (item.reserveType == module.ReserveType.ExternPPT
                    && ((module.ExternPPTReserveDataUnit)item.reserveData).PPTfilePath.CompareTo(filePath) == 0)
                    return false;

            return true;
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
                {
                    reserveList[i].reserveData.ProcessBeforeDeletion();
                    reserveList.RemoveAt(i);
                }
        }

        public ReserveDataUnit[] getSelectionItems()
        {
            List<ReserveDataUnit> list = new List<ReserveDataUnit>();
            foreach (ReserveCollectionUnit item in reserveList)
                if (item.isSelected)
                    list.Add(item.reserveData);
            return list.ToArray();
        }

        /// <summary>
        /// 리스트에서 선택된 항목들의 예약데이터 타입을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public ReserveSelectionsType getTypeOfSelection()
        {
            ReserveType type = ReserveType.NULL;
            int finded = 0;
            for (int i = 0; i < reserveList.Count; i++)
                if (reserveList[i].isSelected)
                {
                    if (finded > 0)
                    {
                        if (type != reserveList[i].reserveType)
                            return ReserveSelectionsType.Mixed;
                    }
                    else
                        type = reserveList[i].reserveType;
                    finded++;
                }

            if (finded == 0)
                return ReserveSelectionsType.NoneSelected;
            
            return RTypeToRSType(type,(finded > 1));
        }
        ReserveSelectionsType RTypeToRSType(ReserveType type, bool isMulti)
        {
            if (isMulti)
                switch (type)
                {
                    case ReserveType.NULL:
                        return ReserveSelectionsType.NULL_Multi;
                    case ReserveType.Bible:
                        return ReserveSelectionsType.Bible_Multi;
                    case ReserveType.Reading:
                        return ReserveSelectionsType.Reading_Multi;
                    case ReserveType.Song:
                        return ReserveSelectionsType.Song_Multi;
                    case ReserveType.ExternPPT:
                        return ReserveSelectionsType.ExternPPT_Multi;
                }
            else
                switch (type)
                {
                    case ReserveType.NULL:
                        return ReserveSelectionsType.NULL_Single;
                    case ReserveType.Bible:
                        return ReserveSelectionsType.Bible_Single;
                    case ReserveType.Reading:
                        return ReserveSelectionsType.Reading_Single;
                    case ReserveType.Song:
                        return ReserveSelectionsType.Song_Single;
                    case ReserveType.ExternPPT:
                        return ReserveSelectionsType.ExternPPT_Single;
                }
            return ReserveSelectionsType.NoneSelected;
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

        public string BackColor
        {
            get { return RTypeToColor(this.reserveType); }
        }
        string RTypeToColor(ReserveType type)
        {
            switch (type)
            {
                case ReserveType.Bible:
                    return "#FF99D9EA";
                case ReserveType.Reading:
                    return "#FFEFE4B0";
                case ReserveType.Song:
                    return "#FFCEEF76";
                case ReserveType.ExternPPT:
                    return "#FFC8BFE7";
                case ReserveType.NULL:
                default:
                    return "#FFC3C3C3";
            }
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
