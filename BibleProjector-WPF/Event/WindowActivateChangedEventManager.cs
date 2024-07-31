using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class WindowActivateChangedEventManager
    {
        public delegate void WindowActivateChanged(bool isActivated);

        public WindowActivateChanged windowActivateChanged;

        public void invoke(bool isActivated)
        {
            windowActivateChanged.Invoke(isActivated);
        }
    }
}
