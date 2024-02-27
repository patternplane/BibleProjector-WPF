using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMReserveData : ViewModel
    {
        public string DisplayTitle { get; set; } = null;
        int _MyIdx;
        public int MyIdx 
        {
            get
            {
                return _MyIdx;
            }
            set
            {
                _MyIdx = value;
                updateDisplayTitle();
            }
        }
        public ReserveViewType ViewType { get; set; }
        
        string title;

        public VMReserveData(string title, ReserveViewType type)
        {
            this.title = title;
            updateDisplayTitle();

            this.ViewType = type;
        }

        public void updateDisplayTitle()
        {
            DisplayTitle = MyIdx + ". " + title;
            OnPropertyChanged("DisplayTitle");
        }

        public void pasteDisplayTitle(string copiedTitle)
        {
            this.DisplayTitle = copiedTitle;
            OnPropertyChanged("DisplayTitle");
        }
    }
}
