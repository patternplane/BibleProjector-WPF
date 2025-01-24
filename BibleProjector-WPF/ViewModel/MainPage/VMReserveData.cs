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
        /// <summary>
        /// test
        /// </summary>
        ~VMReserveData()
        {
            Console.WriteLine("예약 VM 삭제");
        }

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
        public module.Data.ShowData Data;
        public ShowContentType? ContentType { get { return Data?.getDataType(); } }
        
        string title;

        public VMReserveData(string title, ReserveViewType type)
        {
            this.title = title;
            this.Data = null;
            this.ViewType = type;

            updateDisplayTitle();
        }

        public VMReserveData(module.Data.ShowData data, ReserveViewType type = ReserveViewType.NormalItem)
        {
            this.Data = data;
            this.title = data.getTitle1() + " " + data.getTitle2();
            this.ViewType = type;

            updateDisplayTitle();
        }

        public void updateDisplayTitle()
        {
            DisplayTitle = MyIdx + ". " + title;
            if (MyIdx < 10)
                DisplayTitle = "0" + DisplayTitle;
            OnPropertyChanged("DisplayTitle");
        }

        public void pasteDisplayTitle(string copiedTitle)
        {
            this.DisplayTitle = copiedTitle;
            OnPropertyChanged("DisplayTitle");
        }
    }
}
