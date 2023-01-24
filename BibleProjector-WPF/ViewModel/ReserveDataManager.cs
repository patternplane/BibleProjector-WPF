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

        // ======================== 바인딩 속성 ======================== 

        ObservableCollection<ReserveCollectionUnit> reserveList;
        public ObservableCollection<ReserveCollectionUnit> ReserveList
        {
            get { return reserveList; }
            private set { reserveList = value; }
        }

        // ======================== 리스트 변경 이벤트 ======================== 

        static event Event.ReserveListChangedEventHandler listUpdated;

        void onListUpdated(Event.ReserveUpdateType changeType)
        {
            if (listUpdated != null)
                listUpdated(this,new Event.ReserveListChangedEventArgs(changeType));
        }

        static public void subscriptToListChange(Event.ReserveListChangedEventHandler handler)
        {
            listUpdated += handler;
        }

        // ======================== 생성자 ======================== 

        public ReserveDataManager()
        {
            instance = this;

            reserveList = new ObservableCollection<ReserveCollectionUnit>();
        }

        // ======================== 메소드 ========================

        public bool ExternPPT_isNotOverlaped(string filePath)
        {
            foreach (ReserveCollectionUnit item in ReserveList)
                if (item.reserveType == module.ReserveType.ExternPPT
                    && System.IO.Path.GetFileName(((module.ExternPPTReserveDataUnit)item.reserveData).PPTfilePath)
                    .CompareTo(System.IO.Path.GetFileName(filePath)) == 0)
                    return false;

            return true;
        }

        public void addReserve(module.ReserveDataUnit reserveData)
        {
            reserveList.Add(new ReserveCollectionUnit(reserveData));
            onListUpdated(new Event.ReserveTypeConverter().RTToRUT(reserveData.reserveType));
        }

        public void moveSelectionUp()
        {
            Event.ReserveUpdateType type = Event.ReserveUpdateType.None;
            Event.ReserveTypeConverter typeConverter = new Event.ReserveTypeConverter();

            for (int i = 0; i < reserveList.Count; i++)
                if (reserveList[i].isSelected) 
                {
                    if (i == 0)
                        break;

                    type = type | typeConverter.RTToRUT(reserveList[i].reserveType);

                    reserveList.Insert(i - 1, reserveList[i]);
                    reserveList.RemoveAt(i+1);
                }

            onListUpdated(type);
        }

        public void moveUpInCategory(Collection<ReserveCollectionUnit> Items)
        {
            List<ReserveCollectionUnit> filteredList = reserveList.Where(item => (item.reserveType == Items.First().reserveType)).ToList();

            int firstItem = filteredList.IndexOf(Items.First());
            if (firstItem == 0)
                return;

            int preItemIndex = reserveList.IndexOf(filteredList[firstItem - 1]);
            int currentItemIndex;
            bool isSeleted1, isSeleted2;
            for (int i = 0; i < Items.Count; i++)
            {
                currentItemIndex = reserveList.IndexOf(Items[i]);

                isSeleted1 = reserveList[preItemIndex].isSelected;
                isSeleted2 = reserveList[currentItemIndex].isSelected;

                ReserveCollectionUnit temp = reserveList[preItemIndex];
                reserveList[preItemIndex] = reserveList[currentItemIndex];
                reserveList[currentItemIndex] = temp;

                reserveList[currentItemIndex].isSelected = isSeleted1;
                reserveList[preItemIndex].isSelected = isSeleted2;

                preItemIndex = currentItemIndex;
            }

            onListUpdated(new Event.ReserveTypeConverter().RTToRUT(Items.First().reserveType));
        }

        public void moveSelectionDown()
        {
            Event.ReserveUpdateType type = Event.ReserveUpdateType.None;
            Event.ReserveTypeConverter typeConverter = new Event.ReserveTypeConverter();

            for (int i = reserveList.Count-1; i >= 0; i--)
                if (reserveList[i].isSelected)
                {
                    if (i == reserveList.Count-1)
                        break;

                    type = type | typeConverter.RTToRUT(reserveList[i].reserveType);

                    reserveList.Insert(i + 2, reserveList[i]);
                    reserveList.RemoveAt(i);
                }

            onListUpdated(type);
        }

        public void moveDownInCategory(Collection<ReserveCollectionUnit> Items)
        {
            List<ReserveCollectionUnit> filteredList = reserveList.Where(item => (item.reserveType == Items.First().reserveType)).ToList();

            int lastItem = filteredList.IndexOf(Items.Last());
            if (lastItem == (filteredList.Count - 1))
                return;

            int preItemIndex = reserveList.IndexOf(filteredList[lastItem + 1]);
            int currentItemIndex;
            bool isSeleted1, isSeleted2;
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                currentItemIndex = reserveList.IndexOf(Items[i]);

                isSeleted1 = reserveList[preItemIndex].isSelected;
                isSeleted2 = reserveList[currentItemIndex].isSelected;

                ReserveCollectionUnit temp = reserveList[preItemIndex];
                reserveList[preItemIndex] = reserveList[currentItemIndex];
                reserveList[currentItemIndex] = temp;

                reserveList[currentItemIndex].isSelected = isSeleted1;
                reserveList[preItemIndex].isSelected = isSeleted2;

                preItemIndex = currentItemIndex;
            }

            onListUpdated(new Event.ReserveTypeConverter().RTToRUT(Items.First().reserveType));
        }

        public void deleteSelection()
        {
            Event.ReserveUpdateType type = Event.ReserveUpdateType.None;
            Event.ReserveTypeConverter typeConverter = new Event.ReserveTypeConverter();

            for (int i = reserveList.Count - 1; i >= 0; i--)
                if (reserveList[i].isSelected)
                {
                    type = type | typeConverter.RTToRUT(reserveList[i].reserveType);

                    reserveList[i].reserveData.ProcessBeforeDeletion();
                    reserveList.RemoveAt(i);
                }

            onListUpdated(type);
        }

        public void deleteItems(Collection<ReserveCollectionUnit> Items)
        {
            Event.ReserveUpdateType type = Event.ReserveUpdateType.None;
            Event.ReserveTypeConverter typeConverter = new Event.ReserveTypeConverter();

            int selectedIndex;
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                selectedIndex = reserveList.IndexOf(Items[i]);

                type = type | typeConverter.RTToRUT(reserveList[selectedIndex].reserveType);

                reserveList[selectedIndex].reserveData.ProcessBeforeDeletion();
                reserveList.RemoveAt(selectedIndex);
            }

            onListUpdated(type);
        }

        public void deleteItemsByData(object data)
        {
            Collection<ReserveCollectionUnit> deleteList = new Collection<ReserveCollectionUnit>();

            foreach (ReserveCollectionUnit item in reserveList)
                if (item.reserveData.isSameData(data))
                    deleteList.Add(item);

            deleteItems(deleteList);
        }

        public void deleteItemsByData(Collection<object> dataList)
        {
            Collection<ReserveCollectionUnit> deleteList = new Collection<ReserveCollectionUnit>();
            
            foreach (ReserveCollectionUnit item in reserveList)
                foreach (object data in dataList)
                    if (item.reserveData.isSameData(data))
                        deleteList.Add(item);

            deleteItems(deleteList);
        }

        public ReserveCollectionUnit[] getSelectionItems()
        {
            List<ReserveCollectionUnit> list = new List<ReserveCollectionUnit>();
            foreach (ReserveCollectionUnit item in reserveList)
                if (item.isSelected)
                    list.Add(item);
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
        // ============================= 속성 ====================================

        public module.ReserveDataUnit reserveData = null;

        public String DisplayInfo
        {
            get { return completedDisplayInfo(); }
            set { }
        }
        string completedDisplayInfo()
        {
            return "(" + reserveTypeToString(reserveData.reserveType) + ") " + reserveData.getContentInfo();
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
                default:
                    return "오류";
        }
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
            set { _isSelected = value; OnPropertyChanged(nameof(isSelected)); }
        }

        public string BackColor
        {
            get { return RTypeToColor(this.reserveType); }
        }
        public string ForeColor
        {
            get { return RTypeToColor2(this.reserveType); }
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
        string RTypeToColor2(ReserveType type)
        {
            switch (type)
            {
                case ReserveType.Bible:
                    return "#FF69a9bA";
                case ReserveType.Reading:
                    return "#FFbFb480";
                case ReserveType.Song:
                    return "#FF9EbF46";
                case ReserveType.ExternPPT:
                    return "#FF988Fb7";
                case ReserveType.NULL:
                default:
                    return "#FFC3C3C3";
            }
        }

        // ================================ 데이터 생성 및 변경 ================================

        public void ChangeReserveData(module.ReserveDataUnit data)
        {
            this.reserveData = data;
            OnPropertyChanged("");
        }

        public ReserveCollectionUnit(module.ReserveDataUnit reserveDataUnit)
        {
            this.reserveData = reserveDataUnit;
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
