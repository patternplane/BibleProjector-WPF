using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class CapsLockEventManager
    {
        bool isCapsLockDown = false;

        public event Event.KeyStateChangedEventHandler CapsLockStateChanged;

        public void invokeCapsLockChange(bool CapsLockState)
        {
            if (isCapsLockDown != CapsLockState)
            {
                isCapsLockDown = CapsLockState;
                CapsLockStateChanged.Invoke(this, new Event.KeyStateChangedEventArgs(isCapsLockDown));
            }
        }
    }
}
