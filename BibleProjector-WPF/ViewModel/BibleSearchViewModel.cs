using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleSearchViewModel : INotifyPropertyChanged
    {

        string searchText_in = "";
        public string searchText { get { return searchText_in; } set { searchText_in = value; } }

        public ObservableCollection<string> searchList { get; set; } = new ObservableCollection<string>(new string[] { "hello"});

        public bool popupOpen { get; set; } = false;

        // ====================================== 메소드

        public void searchTextChanged()
        {
            

            popupOpen = true;
            NotifyPropertyChanged("popupOpen");
        }

        // ====================================== INotifyPropertyChanged 인터페이스 구현

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
