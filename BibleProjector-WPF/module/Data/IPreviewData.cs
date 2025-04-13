using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public interface IPreviewData
    {
        string getdisplayName(bool isModified);
        string getPreviewContent();
        ShowData getData();
    }
}
