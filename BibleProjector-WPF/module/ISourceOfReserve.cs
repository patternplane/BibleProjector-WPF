﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public interface ISourceOfReserve
    {
        Data.ShowData getItemByReserveInfo(int ReserveInfo);
        int getReserveInfoByItem(Data.ShowData item);
    }
}
