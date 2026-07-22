using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Reserve
{
    public class SaveData
    {
        public ReserveType type { get; set; }
        public int dataCode { get; set; }
        public string framePath { get; set; }

        public SaveData() { }

        public SaveData(ReserveType type, int dataCode, string framePath)
        {
            this.type = type;
            this.dataCode = dataCode;
            this.framePath = framePath;
        }
    }
}
