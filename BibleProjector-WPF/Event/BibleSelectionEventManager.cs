using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public class BibleSelectionEventManager
    {
        public delegate void SelectBible(int book, int chapter, int verse);
        public SelectBible BibleSelectionEvent;

        public void InvokeBibleSelection(int book, int chapter, int verse)
        {
            BibleSelectionEvent(book, chapter, verse);
        }
    }
}
