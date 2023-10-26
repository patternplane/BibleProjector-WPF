using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class BibleSearchData : INotifyPropertyChanged
    {

        public BibleSearchData(string Kuen, string Jang, string Jeul)
        {
            this.Kuen = Kuen;
            this.Jang = Jang;
            this.Jeul = Jeul;

            displayData = Database.getTitle(Kuen);
            if (Jang != null)
                displayData += " " + int.Parse(Jang) + "장 ";
            if (Jeul != null)
                displayData += int.Parse(Jeul) + "절";
        }

        public string Kuen;
        public string Jang;
        public string Jeul;
        string displayData_i;
        public string displayData { get { return displayData_i; } set { displayData_i = value; } }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
