using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleReserveData
    {
        // 성경 예약 단위
        public class BibleReserveContent : INotifyPropertyChanged
        {
            private string Book_in;
            public string Book { get { return Book_in; } set { Book_in = value; NotifyPropertyChanged(); } }
            private string Chapter_in;
            public string Chapter { get { return Chapter_in; } set { Chapter_in = value; NotifyPropertyChanged(); } }
            private string Verse_in;
            public string Verse { get { return Verse_in; } set { Verse_in = value; NotifyPropertyChanged(); } }
            private string DisplayData_in;
            public string DisplayData { get { return DisplayData_in; } private set { DisplayData_in = value; NotifyPropertyChanged(); } }

            public BibleReserveContent(string Book, string Chapter, string Verse)
            {
                this.Book = Book;
                this.Chapter = Chapter;
                this.Verse = Verse;

                this.DisplayData = Database.getTitle(Book)
                        + " "
                        + int.Parse(Chapter)
                        + "장 "
                        + int.Parse(Verse)
                        + "절";
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        // 성경 예약 리스트
        public BindingList<BibleReserveContent> BibleReserveList;

        public BibleReserveData()
        {
            BibleReserveList = module.ProgramData.BibleReserveList;
        }
    }
}
