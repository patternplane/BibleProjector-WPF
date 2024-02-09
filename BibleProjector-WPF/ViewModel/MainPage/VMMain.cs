using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMMain : NotifyPropertyChanged
    {
        public ICommand CPullOptionBar { get; set; }
        public ICommand CPushOptionBar { get; set; }

        bool _isOptionBarOut;
        public bool isOptionBarOut
        {
            get
            {
                return _isOptionBarOut;
            }
            set
            {
                _isOptionBarOut = value;
                OnPropertyChanged("isOptionBarOut");
            }
        }

        public VMMain()
        {
            CPullOptionBar = new RelayCommand(param => pullOptionBar());
            CPushOptionBar = new RelayCommand(param => pushOptionBar());
        }

        void pullOptionBar()
        {
            isOptionBarOut = true;
        }

        void pushOptionBar()
        {
            isOptionBarOut = false;
        }
    }
}
