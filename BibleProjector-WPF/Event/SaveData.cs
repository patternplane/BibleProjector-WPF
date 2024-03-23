using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class SaveDataEventArgs : EventArgs
    {
        public DoSaveData saveDataFunc;
        public SaveDataEventArgs(DoSaveData saveDataFunc)
        {
            this.saveDataFunc = saveDataFunc;
        }
    }

    public delegate void DoSaveData(module.SaveDataTypeEnum type, string data);
    public delegate void SaveDataEventHandler(object sender, SaveDataEventArgs e);
}
