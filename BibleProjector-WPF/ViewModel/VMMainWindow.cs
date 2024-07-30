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

        public KeyInputEventManager keyInputEventManager { get; set; }

        public VMMainWindow(ViewModel vmMain, KeyInputEventManager keyInputEventManager)
        {
            this.VM_Main = vmMain;
            this.keyInputEventManager = keyInputEventManager;
        }
    }
}
