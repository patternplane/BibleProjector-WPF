using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class Manual
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Manual(string title, string content)
        {
            this.Title = title;
            this.Content= content;
        }
    }

    internal class HelpTextData
    {
        const string SEPERATOR = "??^?^";
        const string NEWLINE = "\r\n";

        public Manual[] getManuals()
		{
            string[] rawManualData = ProgramData.getManualData().Split(new string[] { SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
            Manual[] manuals = new Manual[rawManualData.Length];

            string[] manualDetail;
            int i = 0;
            foreach (string data in rawManualData)
            {
                manualDetail = data.Split(new string[] { NEWLINE }, 2,StringSplitOptions.RemoveEmptyEntries);
                manuals[i++] = new Manual(manualDetail[0], manualDetail[1]);
            }

            return manuals;
		}
    }
}
