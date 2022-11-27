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
using System.Windows;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel
{
    internal class ReserveManagerViewModel : INotifyPropertyChanged
    {
        // 이거 말고 더 좋은 방법 있을텐데
        static public ReserveManagerViewModel instance;

        public ReserveManagerViewModel()
        {
            ReserveManagerViewModel.instance = this;

            reserveDataManager = new ReserveDataManager();
            
            // 옵션 탭
            ROViewModels[(int)ReserveType.NULL] = new ReserveOptionViewModels.Null();
            ROViewModels[(int)ReserveType.Bible] = new ReserveOptionViewModels.Bible();
            ROViewModels[(int)ReserveType.Reading] = new ReserveOptionViewModels.Reading();
            ROViewModels[(int)ReserveType.Song] = new ReserveOptionViewModels.Song();
            ROViewModels[(int)ReserveType.ExternPPT] = new ReserveOptionViewModels.ExternPPT();
            ReserveOptionViewModel = ROViewModels[(int)ReserveType.NULL];

            // 프로그램 저장데이터 불러오기
            makeListFromSaveData();

            // 리스트 테스트용 항목들
            /*reserveDataManager.addReserve(new module.EmptyReserveDataUnit());
            reserveDataManager.addReserve(new module.BibleReserveDataUnit("05","003","005"));
            reserveDataManager.addReserve(new module.BibleReserveDataUnit("15", "006", "010"));
            reserveDataManager.addReserve(new module.BibleReserveDataUnit("1", "007", "015"));*/
        }

        // ===================================== 저장 데이터 처리 ====================================
        
        // 프로그램 불러온 저장데이터 가공
        bool checkBeforeMakeReserve(ReserveType type, string SaveData)
        {
            if (type == ReserveType.ExternPPT)
            {
                if ((new ExternPPTManager().checkAvailPPT(SaveData, reserveDataManager.ExternPPT_isNotOverlaped)) != 0)
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }
        void makeListFromSaveData()
        {
            string[] rawData = module.ProgramData.getReserveData(this)
                .Split(new string[] { "§", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            ReserveType type;
            for (int i = 0; i < rawData.Count(); i += 2)
            {
                type = (ReserveType)int.Parse(rawData[i]);
                if (checkBeforeMakeReserve(type, rawData[i + 1]))
                    AddReserveData(
                        ReserveDataUnit.ReserveDataUnitFactory(
                            type,
                            rawData[i + 1]));
            }
        }

        // 프로그램 종료시 저장값 생성 메소드
        public string getSaveData()
        {
            StringBuilder str = new StringBuilder(50);
            foreach (ReserveCollectionUnit item in ReserveDataManager.ReserveList)
            {
                str.Append(((int)item.reserveType));
                str.Append("§");
                str.Append(item.reserveData.getFileSaveText());
                str.Append("\r\n");
            }
            return str.ToString();
        }

        // ===================================== 바인딩 속성 ====================================

        ReserveDataManager reserveDataManager;
        public ReserveDataManager ReserveDataManager
        {
            get { return reserveDataManager; }
            private set { reserveDataManager = value; }
        }

        IReserveOptionViewModel[] ROViewModels = new IReserveOptionViewModel[5];
        IReserveOptionViewModel reserveOptionViewModel;
        public IReserveOptionViewModel ReserveOptionViewModel
        {
            get { return reserveOptionViewModel; } 
            set { reserveOptionViewModel = value;
                OnPropertyChanged("ReserveOptionViewModel");
            }
        }

        ReserveSelectionsType _selectionType = ReserveSelectionsType.NULL_Single;
        public ReserveSelectionsType selectionType { get { return _selectionType; } set { _selectionType = value; OnPropertyChanged("selectionType"); } }

        // ===================================== 메소드 ====================================

        public void startShow()
        {
        }

        public void AddReserveData(module.ReserveDataUnit data)
        {
            reserveDataManager.addReserve(data);
        }

        public void DeleteReserveData()
        {
            if (getTypeOfSelection() == ReserveSelectionsType.NoneSelected)
                return;

            if (MessageBox.Show("선택된 예약항목들을 리스트에서 삭제하시겠습니까?", "예약 항목 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;

            reserveDataManager.deleteSelection();
        }

        public void PutUpReserveData()
        {
            if (getTypeOfSelection() == ReserveSelectionsType.NoneSelected)
                return;
            
            reserveDataManager.moveSelectionUp();
        }

        public void PutDownReserveData()
        {
            if (getTypeOfSelection() == ReserveSelectionsType.NoneSelected)
                return;

            reserveDataManager.moveSelectionDown();
        }

        public ReserveCollectionUnit[] getSelectionItems()
        {
            return reserveDataManager.getSelectionItems();
        }

        public ReserveSelectionsType getTypeOfSelection()
        {
            return ReserveDataManager.getTypeOfSelection();
        }

        public void ListSelectionChanged()
        {
            ReserveSelectionsType type = ReserveDataManager.getTypeOfSelection();
            switch (type)
            {
                case ReserveSelectionsType.Bible_Single:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.Bible];
                    break;
                case ReserveSelectionsType.Reading_Single:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.Reading];
                    break;
                case ReserveSelectionsType.Song_Single:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.Song];
                    break;
                case ReserveSelectionsType.ExternPPT_Single:
                case ReserveSelectionsType.ExternPPT_Multi:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.ExternPPT];
                    break;
                default :
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.NULL];
                    break;
            }

            ReserveOptionViewModel.GiveSelection(getSelectionItems());
            selectionType = type;
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
