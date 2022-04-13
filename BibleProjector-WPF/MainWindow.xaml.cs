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
        Database db;
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

        public MainWindow()
        {
            db = new Database();
            ppt = new Powerpoint();

            InitializeComponent();

            BibleInitialize();
        }

        public void programOut()
        {
            db.Disconnect();
        }

        void BibleInitialize()
        {
            VM_BibleSelectData = new ViewModel.BibleSelectData(db);

            SetBibleButtons();
            SetBibleChapterVerse();

            Bible_CurrentDisplayTextBoxies_Grid.DataContext = VM_BibleSelectData;
        }

        //========================================= 성경 ============================================

        // 세팅

        void SetBibleButtons()
        {
            OldTestButtons = new BindingList<BibleTitleData>();
            Bible_OldTestButtons_ItemsControl.ItemsSource = OldTestButtons;

            NewTestButtons = new BindingList<BibleTitleData>();
            Bible_NewTestButtons_ItemsControl.ItemsSource = NewTestButtons;

            BibleTitleData[] titleData = db.getBibleTitleData();

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
            Bible_Verse_ListBox.SelectionChanged += VerseListBox_SelectedChanged;
        }

        // 성경선택버튼 조작
        void BibleButton_Click(object sender, RoutedEventArgs e)
        {
            string BibleNumber = ((Button)sender).Tag.ToString().PadLeft(2, '0');

            ChapterNumberList.Clear();
            for (int i = 1, chapterCount = db.getChapterCount(BibleNumber); i <= chapterCount; i++)
                ChapterNumberList.Add(i);
            VerseNumberList.Clear();

            VM_BibleSelectData.Book = BibleNumber;
            VM_BibleSelectData.Chapter = "";
            VM_BibleSelectData.Verse = "";
        }

        // 성경 장 선택 조작
        void ChapterListBox_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox chapterListbox = (ListBox)sender;
            if (chapterListbox.SelectedIndex != -1)
            {
                string chapterNumber = (chapterListbox.SelectedIndex + 1).ToString("000");
                VM_BibleSelectData.Chapter = chapterNumber;
                VM_BibleSelectData.Verse = "";

                VerseNumberList.Clear();
                for (int i = 1, verseCount = db.getVerseCount(VM_BibleSelectData.Book + chapterNumber); i <= verseCount; i++)
                    VerseNumberList.Add(i);
            }
        }

        // 성경 절 선택 조작
        void VerseListBox_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox verseListbox = (ListBox)sender;
            if (verseListbox.SelectedIndex != -1)
            {
                string verseNumber = (verseListbox.SelectedIndex + 1).ToString("000");
                VM_BibleSelectData.Verse = verseNumber;
            }
        }
    }

}
