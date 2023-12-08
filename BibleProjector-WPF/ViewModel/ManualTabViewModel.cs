using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    internal class ManualTabViewModel : NotifyPropertyChanged
    {
        public module.Manual[] ManualList { get; set; }

        public ManualTabViewModel()
        {
            ManualList = new module.ManualData().GetManuals();
            OnPropertyChanged(nameof(ManualList));
        }
    }
}
