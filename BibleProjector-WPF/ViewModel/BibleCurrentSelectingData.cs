using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel
{
    class BibleCurrentSelectingData
    {
        public string Book = "";
        public string Chapter = "";
        public string Verse = "";
        public bool isBibleSelected()
        {
            if (Verse.CompareTo("") == 0)
                return false;
            return true;
        }
    }
}
