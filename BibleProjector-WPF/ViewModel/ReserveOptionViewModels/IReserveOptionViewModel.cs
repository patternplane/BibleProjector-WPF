﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    internal interface IReserveOptionViewModel
    {
        void GiveSelection(ReserveCollectionUnit[] data);
        void ShowContent();
    }
}
