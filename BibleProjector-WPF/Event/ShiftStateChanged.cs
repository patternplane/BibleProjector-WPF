using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class KeyStateChangedEventArgs : EventArgs
    {
        public bool KeyOn { get; }

        public KeyStateChangedEventArgs(bool KeyOn)
        {
            this.KeyOn = KeyOn;
        }
    }

    public delegate void KeyStateChangedEventHandler(object sender, KeyStateChangedEventArgs e);
}
