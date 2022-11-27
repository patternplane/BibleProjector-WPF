using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal class Null : IReserveOptionViewModel
    {
        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            return;
        }
    }
}
