using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    internal class ManualTabViewModel : INotifyPropertyChanged
    {
        public module.Manual[] ManualList { get; set; }

        public ManualTabViewModel()
        {
            ManualList = new module.HelpTextData().getManuals();
            OnPropertyChanged(nameof(ManualList));
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
