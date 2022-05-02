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
        // 교독문

        // 교독문 리스트
        BindingList<String> ReadingList;

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



        // 컨트롤
        BibleControl Ctrl_Bible = null;
        ReadingControl Ctrl_Reading = null;

        // =================================================== 프로그램 시작 처리 ======================================================

        public MainWindow()
        {
            Database.DatabaseInitailize();
            module.ProgramOption.Initialize();
            string error = Powerpoint.Initialize();
            if (error.CompareTo("") != 0)
                MessageBox.Show("다음을 확인해주세요 : \r\n" + error, "ppt틀 등록되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);

            InitializeComponent();

            BibleInitialize();
            ReadingInitialize();
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

        void ReadingInitialize()
        {
            SetReadingList();
        }

        // =================================================== 프로그램 종료 처리 ======================================================

        ~MainWindow()
        {
            programOut();
        }

        public void programOut()
        {
            module.ProgramData.saveProgramData();
            Powerpoint.FinallProcess();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Ctrl_Bible != null)
                Ctrl_Bible.ForceClose();
            if (Ctrl_Reading != null)
                Ctrl_Reading.ForceClose();
            if (LyricControl.Ctrl_Song != null)
                LyricControl.Ctrl_Song.ForceClose();
            
            base.OnClosing(e);
        }

        // ========================================= 교독문 ============================================

        // ======================================== 세팅

        void SetReadingList()
        {
            ReadingList = new BindingList<string>(Database.getReadingTitles());
            ReadingListBox.ItemsSource = ReadingList;
        }

        // ========================================= 성경 ============================================

        // ======================================== 세팅

        void SetBibleButtons()
        {
            OldTestButtons = new BindingList<BibleTitleData>();
            Bible_OldTestButtons_ItemsControl.ItemsSource = OldTestButtons;

            NewTestButtons = new BindingList<BibleTitleData>();
            Bible_NewTestButtons_ItemsControl.ItemsSource = NewTestButtons;

            BibleTitleData[] titleData = Database.getBibleTitlesData();

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

        void BibleReserveDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BibleReserveListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in BibleReserveListBox.SelectedItems)
                itemindex.Add(BibleReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            for (int i = itemindex.Count-1; i >= 0 ; i--)
                VM_BibleReserveData.BibleReserveList.Remove(VM_BibleReserveData.BibleReserveList[itemindex[i]]);
        }

        void BibleReserveItemUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (BibleReserveListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in BibleReserveListBox.SelectedItems)
                itemindex.Add(BibleReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            if (itemindex[0] == 0)
                return;

            int i = 0;
            int moveItem;
            int desIndex;
            while ( i < itemindex.Count)
            {
                moveItem = itemindex[i] - 1;
                desIndex = itemindex[i];
                for (;i < itemindex.Count && desIndex == itemindex[i]; i++, desIndex++);

                VM_BibleReserveData.BibleReserveList.Insert(desIndex, VM_BibleReserveData.BibleReserveList[moveItem]);
                VM_BibleReserveData.BibleReserveList.RemoveAt(moveItem);
            }
        }

        void BibleReserveItemDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (BibleReserveListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in BibleReserveListBox.SelectedItems)
                itemindex.Add(BibleReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            if (itemindex.Last() == VM_BibleReserveData.BibleReserveList.Count -1 )
                return;

            int i = itemindex.Count-1;
            int moveItem;
            int desIndex;
            while (i >= 0)
            {
                moveItem = itemindex[i] + 1;
                desIndex = itemindex[i];
                for (i--; i >= 0 && desIndex == itemindex[i]; i--, desIndex--);

                VM_BibleReserveData.BibleReserveList.Insert(desIndex, VM_BibleReserveData.BibleReserveList[moveItem]);
                VM_BibleReserveData.BibleReserveList.RemoveAt(moveItem + 1);
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

        // ======================================== 성경 출력 처리

        void BibleOutputButton_Click(object sender, RoutedEventArgs e)
        {
            if (module.ProgramOption.BibleFramePath == null)
                MessageBox.Show("성경 출력 틀ppt를 등록해주세요!", "ppt틀 등록되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (VM_BibleSelectData.Verse.CompareTo("") == 0)
                MessageBox.Show("출력할 성경구절을 선택해주세요!", "성경 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Ctrl_Bible == null)
                    Ctrl_Bible = new BibleControl(VM_BibleSelectData.Book + VM_BibleSelectData.Chapter + VM_BibleSelectData.Verse);
                else
                    Ctrl_Bible.ShowBible(VM_BibleSelectData.Book + VM_BibleSelectData.Chapter + VM_BibleSelectData.Verse);
                Ctrl_Bible.Show();
            }
        }

        // ======================================== 교독문 처리

        void ReadingOutputButton_Click(object sender, RoutedEventArgs e)
        {
            if (module.ProgramOption.ReadingFramePath == null)
                MessageBox.Show("교독문 출력 틀ppt를 등록해주세요!", "ppt틀 등록되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (ReadingListBox.SelectedIndex == -1)
                MessageBox.Show("출력할 교독문을 선택해주세요!", "교독문 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Ctrl_Reading == null)
                    Ctrl_Reading = new ReadingControl(ReadingListBox.SelectedIndex);
                else
                    Ctrl_Reading.ShowReading(ReadingListBox.SelectedIndex);
                Ctrl_Reading.Show();
            }
        }

    }
}
