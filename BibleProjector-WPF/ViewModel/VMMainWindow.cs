using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class VMMainWindow
    {
        public ViewModel VM_Main { get; set; }

        public Event.KeyInputEventManager keyInputEventManager { get; set; }
        public Event.WindowActivateChangedEventManager windowActivateChangedEventManager { get; set; }

        public VMMainWindow(ViewModel vmMain, Event.KeyInputEventManager keyInputEventManager, Event.WindowActivateChangedEventManager windowActivateChangedEventManager)
        {
            this.VM_Main = vmMain;
            this.keyInputEventManager = keyInputEventManager;
            this.windowActivateChangedEventManager = windowActivateChangedEventManager;
        }
    }
}
