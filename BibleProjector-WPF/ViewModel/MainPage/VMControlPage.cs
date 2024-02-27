using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMControlPage : ViewModel
    {
        // ========== Inner ViewModels ==========

        public ViewModel VM_ShowControler_top { get; set; }
        public ViewModel VM_ShowControler_bottom { get; set; }
        public ViewModel VM_SearchControl { get; set; }
        public ViewModel VM_ReserveList { get; set; }

        // ========== Gen ===========

        public VMControlPage(ViewModel topShowControler, ViewModel bottomShowControler, ViewModel searchControl, ViewModel reserveList)
        {
            this.VM_ShowControler_top = topShowControler;
            this.VM_ShowControler_bottom = bottomShowControler;
            this.VM_SearchControl = searchControl;
            this.VM_ReserveList = reserveList;
        }
    }
}
