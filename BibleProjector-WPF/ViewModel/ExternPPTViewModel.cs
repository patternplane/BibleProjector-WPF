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
        bool isOvelaped(string fileName)
        {
            return ReserveManagerViewModel.instance.ExternPPT_isNotOverlaped(fileName);
        }

        public void RunAddPPT()
        {
            foreach (string filePath in new module.ExternPPTManager().RegisterNewPPT(isOvelaped))
                ReserveManagerViewModel.instance.AddReserveData(
                    new module.ExternPPTReserveDataUnit(filePath));
        }
    }
}
