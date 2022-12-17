using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.IO;

namespace BibleProjector_WPF.ViewModel
{
    class ExternPPTViewModel
    {
        public void RunAddPPT()
        {
            foreach (string filePath in new module.ExternPPTManager().getNewValidationPPT(
                ReserveDataManager.instance.ExternPPT_isNotOverlaped))
                ReserveDataManager.instance.addReserve(
                    new module.ExternPPTReserveDataUnit(filePath));
        }
    }
}
