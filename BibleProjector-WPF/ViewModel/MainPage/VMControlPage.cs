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

        public Predicate<object> IsLoaded { get; set; }

        // ========== Gen ===========

        public VMControlPage(
            ViewModel[] ShowControlers,
            ViewModel searchControl,
            ViewModel reserveList,
            Collection<ViewModel> externPPTEditButtonVMs,
            KeyDownEventManager keyDownEventManager,
            CapsLockEventManager capsLockEventManager,
            module.ShowStarter showStarter)
        {
            this.ShowController = ShowControlers;
            this.VM_ShowControler_top = ShowControlers[2];
            this.VM_ShowControler_bottom = ShowControlers[0];
            this.VM_SearchControl = searchControl;
            this.VM_ReserveList = reserveList;
            this.ExternPPTEditButtons = externPPTEditButtonVMs;

            keyDownEventManager.KeyDown += KeyInputTask;
            capsLockEventManager.CapsLockStateChanged += CapsLockTask;

            showStarter.ShowStartEvent += WhenShowStarted;
        }

        // ========== Show Controller Changing ===========

        void WhenShowStarted(object sender, Event.ShowStartEventArgs e)
        {
            if (e.showData.getDataType() == ShowContentType.Bible
                && VM_ShowControler_bottom != ShowController[0])
            {
                VM_ShowControler_bottom = ShowController[0];
                OnPropertyChanged("VM_ShowControler_bottom");
                ((VMShowControler)VM_ShowControler_bottom).doViewModelChanged();
            }
            else if (e.showData.getDataType() == ShowContentType.Song
                && VM_ShowControler_bottom != ShowController[1])
            {
                VM_ShowControler_bottom = ShowController[1];
                OnPropertyChanged("VM_ShowControler_bottom");
                ((VMShowControler)VM_ShowControler_bottom).doViewModelChanged();
            }
        }

        void KeyInputTask(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (IsLoaded(null))
                foreach (VMShowControler vm in ShowController)
                    vm.keyInputed(e.Key);
        }

        void CapsLockTask(object sender, Event.KeyStateChangedEventArgs e)
        {
            if (true == e.KeyOn)
                return;

            if (IsLoaded(null))
            {
                if (VM_ShowControler_bottom == ShowController[0])
                    VM_ShowControler_bottom = ShowController[1];
                else
                    VM_ShowControler_bottom = ShowController[0];
                OnPropertyChanged("VM_ShowControler_bottom");
                ((VMShowControler)VM_ShowControler_bottom).doViewModelChanged();
            }
        }
    }
}
