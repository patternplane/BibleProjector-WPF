using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class VMMainWindow : ViewModel
    {
        public ViewModel VM_Main { get; set; }

        public Event.KeyInputEventManager keyInputEventManager { get; set; }
        public Event.WindowActivateChangedEventManager windowActivateChangedEventManager { get; set; }
        public bool activeSetter { get; private set; } = false;

        public VMMainWindow(ViewModel vmMain, Event.KeyInputEventManager keyInputEventManager, Event.WindowActivateChangedEventManager windowActivateChangedEventManager, module.ShowStarter showStarter)
        {
            this.VM_Main = vmMain;
            this.keyInputEventManager = keyInputEventManager;
            this.windowActivateChangedEventManager = windowActivateChangedEventManager;
            showStarter.ShowStartTaskDone += showStarted;
        }

        private void showStarted(object sender, EventArgs e)
        {
            activeSetter = true;
            OnPropertyChanged(nameof(activeSetter));
            activeSetter = false;
            OnPropertyChanged(nameof(activeSetter));
        }
    }
}
