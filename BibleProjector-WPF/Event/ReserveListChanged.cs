using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public enum ReserveUpdateType
    {
        None = 0,
        Bible = (1<<0),
        Reading = (1<<1),
        Song = (1<<2),
        ExternPPT = (1<<3)
    }

    public class ReserveTypeConverter
    {
        public ReserveUpdateType RTToRUT(module.ReserveType type)
        {
            switch (type)
            {
                case module.ReserveType.Bible:
                    return ReserveUpdateType.Bible;
                case module.ReserveType.Reading:
                    return ReserveUpdateType.Reading;
                case module.ReserveType.Song:
                    return ReserveUpdateType.Song;
                case module.ReserveType.ExternPPT:
                    return ReserveUpdateType.ExternPPT;
                default: 
                    return ReserveUpdateType.None;
            }
        }
    }

    public class ReserveListChangedEventArgs : EventArgs
    {
        public ReserveListChangedEventArgs(ReserveUpdateType changeType) { this.changeType = changeType; }

        public ReserveUpdateType changeType;
    }

    public delegate void ReserveListChangedEventHandler(object sender, ReserveListChangedEventArgs e);
}
