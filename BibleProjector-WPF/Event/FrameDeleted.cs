using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class FrameDeletedEventArgs : EventArgs
    {
        public FrameDeletedEventArgs(string framefileFullPath) { this.deletedFrameFileFullPath = framefileFullPath; }

        public string deletedFrameFileFullPath;
    }

    public delegate void FrameDeletedEventHandler(object sender, FrameDeletedEventArgs e);
}
