using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class ReserveData
    {
        public ShowData data;
        public ViewModel.Option.VMSongFrameFile songFrame;

        public ReserveData(ShowData data, ViewModel.Option.VMSongFrameFile songFrame)
        {
            this.data = data;
            this.songFrame = songFrame;
        }
    }
}
