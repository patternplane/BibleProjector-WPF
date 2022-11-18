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

        // 프로그램 불러온 저장데이터 가공
        void makeListFromSaveData()
        {
            string[] rawData = module.ProgramData.getReserveData(this)
                .Split(new string[] { "§", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            ReserveDataUnit data;
            for (int i = 0; i < rawData.Count(); i += 2)
            {
                data = ReserveDataUnit.ReserveDataUnitFactory(
                        (ReserveType)int.Parse(rawData[i]),
                        rawData[i + 1]);
                if (data != null)
                    reserveDataManager.addReserve(data);
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

        ReserveDataManager reserveDataManager;
        public ReserveDataManager ReserveDataManager
        {
            get { return reserveDataManager; }
            private set { reserveDataManager = value; }
        }

        object[] ROViewModels = new object[5];
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
            reserveDataManager.moveSelectionUp();
        }

        public void DownButtonClick()
        {
            reserveDataManager.moveSelectionDown();
        }

        public void DeleteButtonClick()
        {
            reserveDataManager.deleteSelection();
        }

        public void ListKeyInputed(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                reserveDataManager.deleteSelection();
        }

        public void ListSelectionChanged()
        {
            switch (ReserveDataManager.getTypeOfSelection())
            {
                case ReserveType.Bible:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.Bible];
                    break;
                case ReserveType.Reading:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.Reading];
                    break;
                case ReserveType.Song:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.Song];
                    break;
                case ReserveType.ExternPPT:
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.ExternPPT];
                    break;
                default :
                    ReserveOptionViewModel = ROViewModels[(int)ReserveType.NULL];
                    break;
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
