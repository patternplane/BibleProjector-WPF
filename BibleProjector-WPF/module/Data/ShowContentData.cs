using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class ShowContentData
    {
        public object Content;
        public object DisplayData;
        public bool DoHighlight;

        public ShowContentData(object content, object displayData, bool doHighlight)
        {
            this.Content = content;
            this.DisplayData = displayData;
            this.DoHighlight = doHighlight;
        }
    }
}
