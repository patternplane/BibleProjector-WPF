using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class KeyInputEventManager
    {
        public delegate void KeyEventHandling(System.Windows.Input.Key key, bool isDown);

        public KeyEventHandling KeyDown;

        public void invokeKeyInput(System.Windows.Input.Key key, bool isDown)
        {
            KeyDown.Invoke(key, isDown);
        }
    }
}
