using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMMain : ViewModel
    {
        // ========== Inner ViewModels ==========

        public ViewModel VM_MainControl { get; set; }
        public ViewModel VM_OptionBar { get; set; }
        public ViewModel VM_Option { get; set; }
        public ViewModel VM_LyricControl { get; set; }

        // ========== Properties ==========

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

        // ========== Gen ===========

        public VMMain(ViewModel controlViewModel, ViewModel option, ViewModel optionBar, ViewModel lyricControl)
        {
            this.VM_MainControl = controlViewModel;
            this.VM_OptionBar = optionBar;
            this.VM_Option = option;
            this.VM_LyricControl = lyricControl;

            CPullOptionBar = new RelayCommand(param => pullOptionBar());
            CPushOptionBar = new RelayCommand(param => pushOptionBar());
        }

        // ========== Command ===========

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
