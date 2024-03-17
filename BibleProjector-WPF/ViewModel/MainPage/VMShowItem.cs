using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMShowItem : ViewModel 
    {
        public int DisplayIndex { get; set; }
        public object Content { get; set; }
        public bool DoHighlight { get; set; }

        public VMShowItem(int displayIdx,object content, bool doHighlight)
        {
            this.DisplayIndex = displayIdx;
            this.Content = content;
            this.DoHighlight = doHighlight;
        }
    }
}
