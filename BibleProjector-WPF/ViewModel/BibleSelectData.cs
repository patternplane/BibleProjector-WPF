using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;

using System.ComponentModel;

namespace BibleProjector_WPF.ViewModel
{
    class BibleSelectData : NotifyPropertyChanged
    {
        // 현재 선택된 데이터
        private string Book_in;
        public string Book
        {
            get { return Book_in; }
            set
            {
                Book_in = value;
                if (Book_in.CompareTo("") == 0)
                    Book_Display = "";
                else
                    Book_Display = Database.getTitle(Book_in);
            }
        }
        private string Book_Display_in;
        public string Book_Display { get { return Book_Display_in; } set { Book_Display_in = value; OnPropertyChanged(); } }
        private string Chapter_in;
        public string Chapter { get { return Chapter_in; } set { Chapter_in = value; OnPropertyChanged(); } }
        private string Verse_in;
        public string Verse { get { return Verse_in; } set { Verse_in = value; OnPropertyChanged(); } }

        public BibleSelectData()
        {
            Book = "";
            Chapter = "";
            Verse = "";
        }
    }
}
