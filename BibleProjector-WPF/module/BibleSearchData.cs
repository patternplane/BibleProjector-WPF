using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class BibleSearchData : NotifyPropertyChanged
    {

        public BibleSearchData(string Kuen, string Jang, string Jeul, bool isShort, int searchDistance)
        {
            this.Kuen = Kuen;
            this.Jang = Jang;
            this.Jeul = Jeul;
            this.searchDistance = searchDistance;

            if (isShort)
                displayData = string.Format("({0}) {1}", Database.getAbrTitle(Kuen), Database.getTitle(Kuen));
            else
                displayData = Database.getTitle(Kuen);

            if (Jang != null)
                displayData += " " + int.Parse(Jang) + "장 ";
            if (Jeul != null)
                displayData += int.Parse(Jeul) + "절";
        }

        public string Kuen;
        public string Jang;
        public string Jeul;
        public int searchDistance;
        string displayData_i;
        public string displayData { get { return displayData_i; } set { displayData_i = value; } }
    }
}
