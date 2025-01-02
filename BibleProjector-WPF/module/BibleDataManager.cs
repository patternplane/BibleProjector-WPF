using BibleProjector_WPF.module.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class BibleDataManager : ISourceOfReserve
    {
        public ShowData getItemByReserveInfo(int ReserveInfo)
        {
            BibleData data = new BibleData(ReserveInfo/1000000, ReserveInfo/1000%1000, ReserveInfo%1000);
            if (data.isAvailData())
                return data;
            else
                return null;
        }

        public int getReserveInfoByItem(ShowData item)
        {
            BibleData bd = (BibleData)item;
            return bd.book * 1000000 + bd.chapter * 1000 + bd.verse;
        }
    }
}
