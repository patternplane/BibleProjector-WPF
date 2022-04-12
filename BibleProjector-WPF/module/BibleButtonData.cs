using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF
{
    class BibleTitleData
    {
        public string BookTitle { get; set; }
        public int BookNumber { get; set; }

        public BibleTitleData(String ButtonName, int BookNumber)
        {
            this.BookTitle = ButtonName;
            this.BookNumber = BookNumber;
        }
    }
}
