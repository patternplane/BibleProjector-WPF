using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class KeyDownEventManager
    {
        public event System.Windows.Input.KeyEventHandler KeyDown;

        public void invokeKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            KeyDown.Invoke(this, e);
        }
    }
}
