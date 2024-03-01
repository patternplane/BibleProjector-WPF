using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class ShiftStateChangedEventArgs : EventArgs
    {
        public bool ShiftOn { get; }

        public ShiftStateChangedEventArgs(bool ShiftOn)
        {
            this.ShiftOn = ShiftOn;
        }
    }

    public delegate void ShiftStateChangedEventHandler(object sender, ShiftStateChangedEventArgs e);
}
