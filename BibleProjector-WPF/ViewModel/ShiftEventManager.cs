using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class ShiftEventManager
    {
        bool isShifted = false;

        public event Event.KeyStateChangedEventHandler ShiftStateChanged;

        public void invokeShiftChange(bool shiftState)
        {
            if (isShifted != shiftState)
            {
                isShifted = shiftState;
                ShiftStateChanged.Invoke(this, new Event.KeyStateChangedEventArgs(isShifted));
            }
        }
    }
}
