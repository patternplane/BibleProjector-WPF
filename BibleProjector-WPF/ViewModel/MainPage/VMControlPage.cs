using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMControlPage : ViewModel
    {
        // ========== Inner ViewModels ==========

        ViewModel[] ShowController;
        public ViewModel VM_ShowControler_top { get; set; }
        public ViewModel VM_ShowControler_bottom { get; set; }
        public ViewModel VM_SearchControl { get; set; }
        public ViewModel VM_ReserveList { get; set; }

        // ========== Binding ===========

        public Collection<ViewModel> ExternPPTEditButtons { get; }

        // ========== Gen ===========

        public VMControlPage(
            ViewModel[] ShowControlers,
            ViewModel searchControl,
            ViewModel reserveList,
            Collection<ViewModel> externPPTEditButtonVMs,
            CapsLockEventManager capsLockEventManager)
        {
            this.ShowController = ShowControlers;
            this.VM_ShowControler_top = ShowControlers[1];
            this.VM_ShowControler_bottom = ShowControlers[2];
            this.VM_SearchControl = searchControl;
            this.VM_ReserveList = reserveList;
            this.ExternPPTEditButtons = externPPTEditButtonVMs;

            capsLockEventManager.CapsLockStateChanged += CapsLockTask;
        }

        // ========== key Event Handling ===========

        void CapsLockTask(object sender, Event.KeyStateChangedEventArgs e)
        {
            if (true == e.KeyOn)
                return;

            if (VM_ShowControler_top == ShowController[0])
                VM_ShowControler_top = ShowController[1];
            else
                VM_ShowControler_top = ShowController[0];
            OnPropertyChanged("VM_ShowControler_top");
        }
    }
}
