using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class ShowStartEventArgs : EventArgs
    {
        public ShowStartEventArgs(module.Data.ShowData showData) { this.showData = showData; }

        public module.Data.ShowData showData;
    }

    public delegate void ShowStartEventHandler(object sender, ShowStartEventArgs e);
}
