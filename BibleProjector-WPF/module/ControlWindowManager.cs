using BibleProjector_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    internal class ControlWindowManager
    {
        static BibleControl Ctrl_Bible = null;
        static ReadingControl Ctrl_Reading = null;
        static SongControl Ctrl_Song = null;
        static ExternPPTControl Ctrl_ExternPPT = null;

        public BibleControlViewModel showBibleControl()
        {
            if (Ctrl_Bible == null)
                Ctrl_Bible = new BibleControl();
            Ctrl_Bible.Show();

            return (BibleControlViewModel)Ctrl_Bible.DataContext;
        }
        public ReadingControlViewModel showReadingControl()
        {
            if (Ctrl_Reading == null)
                Ctrl_Reading = new ReadingControl();
            Ctrl_Reading.Show();

            return (ReadingControlViewModel)Ctrl_Reading.DataContext;
        }
        public SongControlViewModel showSongControl()
        {
            if (Ctrl_Song == null)
                Ctrl_Song = new SongControl();
            Ctrl_Song.Show();

            return (SongControlViewModel)Ctrl_Song.DataContext;
        }
        public ExternPPTControlViewModel showExternPPTControl()
        {
            if (Ctrl_ExternPPT == null)
                Ctrl_ExternPPT = new ExternPPTControl();
            Ctrl_ExternPPT.Show();

            return (ExternPPTControlViewModel)Ctrl_ExternPPT.DataContext;
        }

        public ExternPPTControlViewModel getExternPPTControlViewModel()
        {
            if (Ctrl_ExternPPT == null)
                return null;
            return (ExternPPTControlViewModel)Ctrl_ExternPPT.DataContext;
        }

        public void ForceClose()
        {
            if (Ctrl_Bible != null)
                Ctrl_Bible.ForceClose();
            if (Ctrl_Reading != null)
                Ctrl_Reading.ForceClose();
            if (Ctrl_ExternPPT != null)
                Ctrl_ExternPPT.ForceClose();
            if (Ctrl_Song != null)
                Ctrl_Song.ForceClose();
        }
    }
}
