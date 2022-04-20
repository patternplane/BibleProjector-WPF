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
            BibleReserveList = getBibleReserveList(module.ProgramData.getBibleReserveData(this));
        }

        // 데이터 받아올 때 입력 규격
        BindingList<BibleReserveContent> getBibleReserveList(string fileContent)
        {
            BindingList<BibleReserveContent> BibleReserveList = new BindingList<BibleReserveContent>();

            string[] data;
            foreach (string token in fileContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                data = token.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                BibleReserveList.Add(new BibleReserveContent(data[0], data[1], data[2]));
            }

            return BibleReserveList;
        }

        // 데이터 저장할 때 출력 규격
        public string getSaveData()
        {
            StringBuilder str = new StringBuilder(50).Clear();
            foreach (BibleReserveContent item in BibleReserveList)
            {
                str.Append(item.Book);
                str.Append(" ");
                str.Append(item.Chapter);
                str.Append(" ");
                str.Append(item.Verse);
                str.Append("\r\n");
            }
            return str.ToString();
        }
    }
}
