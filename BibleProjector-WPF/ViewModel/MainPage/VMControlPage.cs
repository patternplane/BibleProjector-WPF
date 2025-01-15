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
        public ObservableCollection<ViewModel> VM_ShowControler_bottoms { get; private set; }
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
            this.VM_ShowControler_bottoms = new ObservableCollection<ViewModel>() { ShowControlers[0], ShowControlers[1] };
            this.VM_BibleSeletion = bibleSelection;
            this.VM_SearchControl = searchControl;
            this.VM_ReserveList = reserveList;
            this.ExternPPTEditButtons = externPPTEditButtonVMs;

            setBottomShowController(0, false);

            keyInputEventManager.KeyDownWithRepeat += KeyInputTask;

            showStarter.ShowStartPreparing += WhenShowStarted;
        }

        // ========== Show Controller Changing ===========

        void WhenShowStarted(object sender, Event.ShowStartEventArgs e)
        {
            if (e.showData.getDataType() == ShowContentType.Bible)
                setBottomShowController(0, false); // Bible 컨트롤러가 0에 연결되는 것은 너무 변경에 취약함 (개선 필요)
            else if (e.showData.getDataType() == ShowContentType.Song)
                setBottomShowController(1, false); // Song 컨트롤러가 1에 연결되는 것은 너무 변경에 취약함 (개선 필요)
        }

        void KeyInputTask(System.Windows.Input.Key key, bool isDown, bool isRepeat)
        {
            if (IsLoaded(null)) {
                foreach (VMShowControler vm in ShowController)
                    vm.keyInputed(key, isDown, isRepeat);

                if (key == System.Windows.Input.Key.CapsLock
                    && isDown
                    && !isRepeat)
                {
                    setNextBottomShowController(true);
                }
            }
        }

        private int currentSelection = -1;

        private void setNextBottomShowController(bool invokeViewModelChanged)
        {
            setBottomShowController(
                (currentSelection + 1) % VM_ShowControler_bottoms.Count,
                invokeViewModelChanged);
        }
 
        private void setBottomShowController(int index, bool invokeViewModelChanged)
        {
            if (index < 0 || index >= ShowController.Length)
                throw new Exception("잘못된 쇼 컨트롤러 인덱스 : " + index + "번 컨트롤러는 없습니다! / " + ShowController.Length);

            if (currentSelection != index)
            {
                if (0 <= currentSelection && currentSelection < VM_ShowControler_bottoms.Count)
                {
                    ((VMShowControler)VM_ShowControler_bottoms[currentSelection]).hasFocus = false;
                    ((VMShowControler)VM_ShowControler_bottoms[currentSelection]).hide();
                }
                ((VMShowControler)VM_ShowControler_bottoms[index]).show();
                currentSelection = index;
            }

            if (invokeViewModelChanged)
            {
                ((VMShowControler)VM_ShowControler_bottoms[index]).doViewModelChanged();
            }
        }
    }
}
