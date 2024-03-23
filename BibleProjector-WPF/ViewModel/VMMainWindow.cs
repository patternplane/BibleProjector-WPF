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

        public ShiftEventManager shiftEventManager { get; set; }
        public CapsLockEventManager capsLockEventManager { get; set; }

        public VMMainWindow(ViewModel vmMain, ShiftEventManager shiftEventManager, CapsLockEventManager capsLockEventManager)
        {
            this.VM_Main = vmMain;
            this.shiftEventManager = shiftEventManager;
            this.capsLockEventManager = capsLockEventManager;
        }
    }
}
