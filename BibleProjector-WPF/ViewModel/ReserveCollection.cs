using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    public class ReserveCollection
    {
        static public Collection<ReserveCollectionUnit> makeReserveCollection(module.ReserveData reserveData)
        {
            Collection<ReserveCollectionUnit> reserveList = new Collection<ReserveCollectionUnit>();

            foreach (module.ReserveDataUnit dataUnit in reserveData.getReserveList())
                reserveList.Add(new ReserveCollectionUnit(dataUnit));

            return reserveList;
        }
    }

    public class ReserveCollectionUnit
    {
        string displayInfo = "";
        public String DisplayInfo
        {
            get { return displayInfo; }
            set { displayInfo = value; }
        }

        bool _isSelected = false;
        public bool isSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public module.ReserveDataUnit reserveData = null;

        public ReserveCollectionUnit(module.ReserveDataUnit reserveDataUnit)
        {
            this.reserveData = reserveDataUnit;
            this.displayInfo = reserveDataUnit.getDisplayInfo();
        }
    }
}
