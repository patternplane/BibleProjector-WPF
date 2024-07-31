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
        public ViewModel VM_ShowControler_top { get; }
        public ViewModel VM_ShowControler_bottom { get; private set; }
        public ViewModel VM_BibleSeletion { get; }
        public ViewModel VM_SearchControl { get; }
        public ViewModel VM_ReserveList { get; }

        // ========== Binding ===========

        public Collection<ViewModel> ExternPPTEditButtons { get; }

        public Predicate<object> IsLoaded { get; set; }

        // ========== Gen ===========

        public VMControlPage(
            ViewModel[] ShowControlers,
            ViewModel bibleSelection,
            ViewModel searchControl,
            ViewModel reserveList,
            Collection<ViewModel> externPPTEditButtonVMs,
            Event.KeyInputEventManager keyInputEventManager,
            module.ShowStarter showStarter)
        {
            this.ShowController = ShowControlers;
            this.VM_ShowControler_top = ShowControlers[2];
            this.VM_ShowControler_bottom = ShowControlers[0];
            this.VM_BibleSeletion = bibleSelection;
            this.VM_SearchControl = searchControl;
            this.VM_ReserveList = reserveList;
            this.ExternPPTEditButtons = externPPTEditButtonVMs;

            keyInputEventManager.KeyDown += KeyInputTask;

            showStarter.ShowStartEvent += WhenShowStarted;
        }

        // ========== Show Controller Changing ===========

        void WhenShowStarted(object sender, Event.ShowStartEventArgs e)
        {
            if (e.showData.getDataType() == ShowContentType.Bible
                && VM_ShowControler_bottom != ShowController[0])
                changeBottomShowController(0);
            else if (e.showData.getDataType() == ShowContentType.Song
                && VM_ShowControler_bottom != ShowController[1])
                changeBottomShowController(1);
        }

        void KeyInputTask(System.Windows.Input.Key key, bool isDown)
        {
            if (IsLoaded(null)) {
                foreach (VMShowControler vm in ShowController)
                    vm.keyInputed(key, isDown);

                if (key == System.Windows.Input.Key.CapsLock
                    && isDown)
                {
                    if (VM_ShowControler_bottom == ShowController[0])
                        changeBottomShowController(1);
                    else
                        changeBottomShowController(0);
                }
            }
        }

        void changeBottomShowController(int index)
        {
            if (index < 0 || index >= ShowController.Length)
                throw new Exception("잘못된 쇼 컨트롤러 인덱스 : " + index + "번 컨트롤러는 없습니다! / " + ShowController.Length);

            if (VM_ShowControler_bottom != ShowController[index])
            {
                ((VMShowControler)VM_ShowControler_bottom).hasFocus = false;
                VM_ShowControler_bottom = ShowController[index];
            }
            OnPropertyChanged("VM_ShowControler_bottom");
            ((VMShowControler)VM_ShowControler_bottom).doViewModelChanged();
        }
    }
}
