using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

namespace BibleProjector_WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 프로그램 모듈
        Powerpoint ppt;

        // 성경

        // 신구약 버튼들
        BindingList<BibleTitleData> OldTestButtons;
        BindingList<BibleTitleData> NewTestButtons;
        // 장 번호 리스트
        BindingList<int> ChapterNumberList;
        // 절 번호 리스트
        BindingList<int> VerseNumberList;

        // 현재 선택된 데이터
        ViewModel.BibleSelectData VM_BibleSelectData;
        ViewModel.BibleCurrentSelectingData VM_BibleCurrentSelectingData;
        // 예약 정보
        ViewModel.BibleReserveData VM_BibleReserveData;

        // =================================================== 프로그램 시작 처리 ======================================================

        public MainWindow()
        {
            Database.DatabaseInitailize();
            module.ProgramData.getProgramData();
            ppt = new Powerpoint();

            InitializeComponent();

            BibleInitialize();
        }

        void BibleInitialize()
        {
            SetBibleButtons();
            SetBibleChapterVerse();
            SetBibleReserveButtons();

            VM_BibleCurrentSelectingData = new ViewModel.BibleCurrentSelectingData();

            Bible_CurrentDisplayTextBoxies_Grid.DataContext = VM_BibleSelectData = new ViewModel.BibleSelectData();

            VM_BibleReserveData = new ViewModel.BibleReserveData();
            BibleReserveListBox.ItemsSource = VM_BibleReserveData.BibleReserveList;
            BibleReserveListBox.DisplayMemberPath = "DisplayData";
        }

        // =================================================== 프로그램 종료 처리 ======================================================

        public void programOut()
        {
            Database.Disconnect();
            module.ProgramData.saveProgramData();
        }

        //========================================= 성경 ============================================

        // ======================================== 세팅

        void SetBibleButtons()
        {
            OldTestButtons = new BindingList<BibleTitleData>();
            Bible_OldTestButtons_ItemsControl.ItemsSource = OldTestButtons;

            NewTestButtons = new BindingList<BibleTitleData>();
            Bible_NewTestButtons_ItemsControl.ItemsSource = NewTestButtons;

            BibleTitleData[] titleData = Database.getBibleTitleData();

            for (int i = 0; i < 39; i++)
                OldTestButtons.Add(titleData[i]);
            for (int i = 39; i < 66; i++)
                NewTestButtons.Add(titleData[i]);
        }

        void SetBibleChapterVerse()
        {
            ChapterNumberList = new BindingList<int>();
            VerseNumberList = new BindingList<int>();

            Bible_Chapter_ListBox.ItemsSource = ChapterNumberList;
            Bible_Verse_ListBox.ItemsSource = VerseNumberList;

            Bible_Chapter_ListBox.SelectionChanged += ChapterListBox_SelectedChanged;
            Bible_Chapter_ListBox.GotFocus += ChapterListBox_GotFocus;
            Bible_Verse_ListBox.SelectionChanged += VerseListBox_SelectedChanged;
            Bible_Verse_ListBox.GotFocus += VerseListBox_GotFocus;
        }

        void SetBibleReserveButtons()
        {
            BibleReserveAddButton.Click += BibleReserveAddButton_Click;
            BibleReserveDeleteButton.Click += BibleReserveDeleteButton_Click;
            BibleReserveItemUpButton.Click += BibleReserveItemUpButton_Click;
            BibleReserveItemDownButton.Click += BibleReserveItemDownButton_Click;

            BibleReserveListBox.GotFocus += BibleReserveListBox_GotFocus;
            BibleReserveListBox.SelectionChanged += BibleReserveListBox_SelectionChanged;
        }

        // ======================================== 성경 선택 처리

        // 성경선택버튼 조작
        void BibleButton_Click(object sender, RoutedEventArgs e)
        {
            string BibleNumber = ((Button)sender).Tag.ToString().PadLeft(2, '0');

            if (BibleNumber.CompareTo(VM_BibleCurrentSelectingData.Book) != 0)
            {
                ChapterNumberList.Clear();
                for (int i = 1, chapterCount = Database.getChapterCount(BibleNumber); i <= chapterCount; i++)
                    ChapterNumberList.Add(i);
                VerseNumberList.Clear();

                VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book = BibleNumber;
                VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter = "";
                VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse = "";
            }
            else
            {
                VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book;
                VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter;
                VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse;
            }
        }

        // 성경 장 선택 조작
        void ChapterListBox_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox chapterListbox = (ListBox)sender;
            if (chapterListbox.SelectedIndex != -1)
            {
                string chapterNumber = (chapterListbox.SelectedIndex + 1).ToString("000");
                VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book;
                VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter = chapterNumber;
                VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse = "";

                VerseNumberList.Clear();
                for (int i = 1, verseCount = Database.getVerseCount(VM_BibleSelectData.Book + chapterNumber); i <= verseCount; i++)
                    VerseNumberList.Add(i);
            }
        }
        void ChapterListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ListBox chapterListbox = (ListBox)sender;
            if (chapterListbox.SelectedIndex != -1)
            {
                string chapterNumber = (chapterListbox.SelectedIndex + 1).ToString("000");

                if (chapterNumber.CompareTo(VM_BibleCurrentSelectingData.Chapter) != 0)
                {
                    VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book;
                    VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter = chapterNumber;
                    VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse = "";

                    VerseNumberList.Clear();
                    for (int i = 1, verseCount = Database.getVerseCount(VM_BibleSelectData.Book + chapterNumber); i <= verseCount; i++)
                        VerseNumberList.Add(i);
                }
                else
                {
                    VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book;
                    VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter;
                    VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse;
                }
            }
        }

        // 성경 절 선택 조작
        void VerseListBox_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox verseListbox = (ListBox)sender;
            if (verseListbox.SelectedIndex != -1)
            {
                string verseNumber = (verseListbox.SelectedIndex + 1).ToString("000");
                VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book;
                VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter;
                VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse = verseNumber;
            }
        }
        void VerseListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ListBox verseListbox = (ListBox)sender;
            if (verseListbox.SelectedIndex != -1)
            {
                string verseNumber = (verseListbox.SelectedIndex + 1).ToString("000");
                VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book;
                VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter;
                VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse = verseNumber;
            }
        }

        // ======================================== 성경 예약 처리

        void BibleReserveAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM_BibleCurrentSelectingData.isBibleSelected())
                VM_BibleReserveData.BibleReserveList.Add(new ViewModel.BibleReserveData.BibleReserveContent(VM_BibleCurrentSelectingData.Book, VM_BibleCurrentSelectingData.Chapter, VM_BibleCurrentSelectingData.Verse));
        }

        void BibleReserveDeleteButton_Click (object sender, RoutedEventArgs e)
        {
            int count = BibleReserveListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = BibleReserveListBox.Items.IndexOf(BibleReserveListBox.SelectedItems[0]);
            foreach (object item in BibleReserveListBox.SelectedItems)
            {
                if (startidx > BibleReserveListBox.Items.IndexOf(item))
                    startidx = BibleReserveListBox.Items.IndexOf(item);
            }

            for (; count > 0; count--)
                VM_BibleReserveData.BibleReserveList.Remove(VM_BibleReserveData.BibleReserveList[startidx]);
        }

        void BibleReserveItemUpButton_Click(object sender, RoutedEventArgs e)
        {
            int count = BibleReserveListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = BibleReserveListBox.Items.IndexOf(BibleReserveListBox.SelectedItems[0]);
            foreach (object item in BibleReserveListBox.SelectedItems)
            {
                if (startidx > BibleReserveListBox.Items.IndexOf(item))
                    startidx = BibleReserveListBox.Items.IndexOf(item);
            }

            if (startidx != 0)
            {
                VM_BibleReserveData.BibleReserveList.Insert(startidx + count,VM_BibleReserveData.BibleReserveList[startidx - 1]);
                VM_BibleReserveData.BibleReserveList.Remove(VM_BibleReserveData.BibleReserveList[startidx - 1]);
            }
        }

        void BibleReserveItemDownButton_Click(object sender, RoutedEventArgs e)
        {
            int count = BibleReserveListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = BibleReserveListBox.Items.IndexOf(BibleReserveListBox.SelectedItems[0]);
            foreach (object item in BibleReserveListBox.SelectedItems)
            {
                if (startidx > BibleReserveListBox.Items.IndexOf(item))
                    startidx = BibleReserveListBox.Items.IndexOf(item);
            }

            if (startidx + count != VM_BibleReserveData.BibleReserveList.Count)
            {
                ViewModel.BibleReserveData.BibleReserveContent item = VM_BibleReserveData.BibleReserveList[startidx + count];
                VM_BibleReserveData.BibleReserveList.Remove(item);
                VM_BibleReserveData.BibleReserveList.Insert(startidx, item);
            }
        }

        void BibleReserveListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((ListBox)sender).SelectedIndex == -1)
            {/*
                VM_BibleSelectData.Book = "";
                VM_BibleSelectData.Chapter = "";
                VM_BibleSelectData.Verse = "";
            */
            }
            else
            {
                ViewModel.BibleReserveData.BibleReserveContent item = (ViewModel.BibleReserveData.BibleReserveContent)((ListBox)sender).SelectedItem;
                VM_BibleSelectData.Book = item.Book;
                VM_BibleSelectData.Chapter = item.Chapter;
                VM_BibleSelectData.Verse = item.Verse;
            }
        }

        void BibleReserveListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedIndex == -1)
            {/*
                VM_BibleSelectData.Book = "";
                VM_BibleSelectData.Chapter = "";
                VM_BibleSelectData.Verse = "";
            */
            }
            else
            {
                ViewModel.BibleReserveData.BibleReserveContent item = (ViewModel.BibleReserveData.BibleReserveContent)((ListBox)sender).SelectedItem;
                VM_BibleSelectData.Book = item.Book;
                VM_BibleSelectData.Chapter = item.Chapter;
                VM_BibleSelectData.Verse = item.Verse;
            }
        }
    }

}
