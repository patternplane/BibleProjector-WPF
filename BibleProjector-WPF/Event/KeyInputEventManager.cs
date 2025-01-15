using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class KeyInputEventManager
    {
        public delegate void KeyEventHandling(System.Windows.Input.Key key, bool isDown);
        public delegate void KeyEventWithRepeatHandling(System.Windows.Input.Key key, bool isDown, bool isRepeat);

        public KeyEventHandling KeyDown;
        public KeyEventWithRepeatHandling KeyDownWithRepeat;

        public void invokeKeyInput(System.Windows.Input.Key key, bool isDown, bool isRepeat)
        {
            if (!isRepeat)
                KeyDown?.Invoke(key, isDown);
            KeyDownWithRepeat?.Invoke(key, isDown, isRepeat);
        }
    }
}
