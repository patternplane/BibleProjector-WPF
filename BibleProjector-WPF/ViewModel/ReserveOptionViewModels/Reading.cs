using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class Reading : IReserveOptionViewModel,INotifyPropertyChanged
    {
        public Reading() 
        {
            ReadingList = Database.getReadingTitles();
        }

        ReserveCollectionUnit SelectedReserveItem = null;
        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            if (data.Count() == 1)
            {
                dataChanged(((module.ReadingReserveDataUnit)((ReserveCollectionUnit)data[0]).reserveData).readingIndex);
                SelectedReserveItem = (ReserveCollectionUnit)data[0];
            }
            else
            {
                dataChanged(-1);
                SelectedReserveItem = null;
            }
        }

        public string[] ReadingList { get; set; }

        int _SelectionIndex;
        public int SelectionIndex { get { return _SelectionIndex; } set { _SelectionIndex = value; selectionChanged(); } }
        void dataChanged(int ReadingIndex)
        {
            _SelectionIndex = ReadingIndex;
            OnPropertyChanged(nameof(SelectionIndex));
        }
        void selectionChanged()
        {
            SelectedReserveItem.ChangeReserveData(new module.ReadingReserveDataUnit(SelectionIndex));
        }

        public void ReadingRun()
        {
            //여기다 이제 실제 시행
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
