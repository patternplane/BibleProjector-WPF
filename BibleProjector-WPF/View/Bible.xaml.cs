﻿using System;
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
using BibleProjector_WPF.ViewModel;
using System.Collections.ObjectModel;

namespace BibleProjector_WPF
{
    /// <summary>
    /// Bible.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Bible : UserControl
    {
        public static Bible tempBibleAccesser = null;

        public Bible()
        {
            if (tempBibleAccesser == null)
                tempBibleAccesser = this;
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

            BibleReserveListBox.DataContext = VM_BibleReserveData = new ViewModel.BibleReserveData();

            BibleSearchBorder.DataContext = VM_BibleSearch = new ViewModel.BibleSearchViewModel();
        }



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
        // 검색
        ViewModel.BibleSearchViewModel VM_BibleSearch;


        // 성경 수정 창
        public BibleModifyWindow SubWindow_BibleModify = null;

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
            BibleReserveListBox.KeyDown += BibleReserveListBox_KeyDown;
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

        // ======================================== 성경 검색 처리

        void BibleSearchPreKeyUp(object sender, KeyEventArgs e)
        {
            BibleSearchTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            VM_BibleSearch.searchTextChanged();
        }

        // ======================================== 성경 예약 처리

        Collection<ReserveCollectionUnit> getSortedSelection(ItemCollection items, System.Collections.IList rawSelection)
        {
            List<int> itemindex = new List<int>(rawSelection.Count);

            for (int i = 0; i < rawSelection.Count; i++)
                itemindex.Add(items.IndexOf(rawSelection[i]));
            itemindex.Sort();

            Collection<ReserveCollectionUnit> sortedItems = new Collection<ReserveCollectionUnit>();
            foreach (int i in itemindex)
                sortedItems.Add((ReserveCollectionUnit)items[i]);

            return sortedItems;
        }

        void BibleReserveAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM_BibleCurrentSelectingData.isBibleSelected())
            {
                //VM_BibleReserveData.BibleReserveList.Add(new ViewModel.BibleReserveData.BibleReserveContent(VM_BibleCurrentSelectingData.Book, VM_BibleCurrentSelectingData.Chapter, VM_BibleCurrentSelectingData.Verse));

                // 예약창에 적용하는 데이터
                ViewModel.ReserveManagerViewModel.instance.ReserveDataManager.addReserve(
                    new module.BibleReserveDataUnit(VM_BibleCurrentSelectingData.Book, VM_BibleCurrentSelectingData.Chapter, VM_BibleCurrentSelectingData.Verse));
            }
        }

        void BibleReserveDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            BibleReserveDelete();
        }

        void BibleReserveListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                BibleReserveDelete();
            if (e.Key == Key.Enter)
                BibleOutPut();
        }

        void BibleReserveDelete()
        {
            if (BibleReserveListBox.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("선택된 예약구절을 삭제하시겠습니까?", "성경 예약 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            ViewModel.ReserveDataManager.instance.deleteItems(
                getSortedSelection(BibleReserveListBox.Items, BibleReserveListBox.SelectedItems));

            /*List<int> itemindex = new List<int>(10);
            foreach (object item in BibleReserveListBox.SelectedItems)
                itemindex.Add(BibleReserveListBox.Items.IndexOf(item));
            itemindex.Sort();*/

            /*for (int i = itemindex.Count - 1; i >= 0; i--)
                VM_BibleReserveData.BibleReserveList.Remove(VM_BibleReserveData.BibleReserveList[itemindex[i]]);*/
        }

        void BibleReserveItemUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (BibleReserveListBox.SelectedItems.Count == 0)
                return;

            ViewModel.ReserveDataManager.instance.moveUpInCategory(
                getSortedSelection(BibleReserveListBox.Items, BibleReserveListBox.SelectedItems));

            /*List<int> itemindex = new List<int>(10);
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

                *//*VM_BibleReserveData.BibleReserveList.Insert(desIndex, VM_BibleReserveData.BibleReserveList[moveItem]);
                VM_BibleReserveData.BibleReserveList.RemoveAt(moveItem);*//*
            }*/
        }

        void BibleReserveItemDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (BibleReserveListBox.SelectedItems.Count == 0)
                return;

            ViewModel.ReserveDataManager.instance.moveDownInCategory(
                getSortedSelection(BibleReserveListBox.Items, BibleReserveListBox.SelectedItems));

            /*List<int> itemindex = new List<int>(10);
            foreach (object item in BibleReserveListBox.SelectedItems)
                itemindex.Add(BibleReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            *//*if (itemindex.Last() == VM_BibleReserveData.BibleReserveList.Count -1 )
                return;*//*

            int i = itemindex.Count-1;
            int moveItem;
            int desIndex;
            while (i >= 0)
            {
                moveItem = itemindex[i] + 1;
                desIndex = itemindex[i];
                for (i--; i >= 0 && desIndex == itemindex[i]; i--, desIndex--);

                *//*VM_BibleReserveData.BibleReserveList.Insert(desIndex, VM_BibleReserveData.BibleReserveList[moveItem]);
                VM_BibleReserveData.BibleReserveList.RemoveAt(moveItem + 1);*//*
            }*/
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
                module.BibleReserveDataUnit item = (module.BibleReserveDataUnit)(((ViewModel.ReserveCollectionUnit)(((ListBox)sender).SelectedItem)).reserveData);
                VM_BibleSelectData.Book = item.Book;
                VM_BibleSelectData.Chapter = item.Chapter;
                VM_BibleSelectData.Verse = item.Verse;

                applyBibleMoving(item.Book, item.Chapter, item.Verse);
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
                module.BibleReserveDataUnit item = (module.BibleReserveDataUnit)(((ViewModel.ReserveCollectionUnit)(((ListBox)sender).SelectedItem)).reserveData);
                VM_BibleSelectData.Book = item.Book;
                VM_BibleSelectData.Chapter = item.Chapter;
                VM_BibleSelectData.Verse = item.Verse;

                applyBibleMoving(item.Book, item.Chapter, item.Verse);
            }
        }

        // ======================================== 성경 이동 반영 처리

        public void applyBibleMoving(string book, string chapter, string verse)
        {
            ChapterNumberList.Clear();
            for (int i = 1, chapterCount = Database.getChapterCount(book); i <= chapterCount; i++)
                ChapterNumberList.Add(i);
            VerseNumberList.Clear();

            VM_BibleSelectData.Book = VM_BibleCurrentSelectingData.Book = book;
            VM_BibleSelectData.Chapter = VM_BibleCurrentSelectingData.Chapter = "";
            VM_BibleSelectData.Verse = VM_BibleCurrentSelectingData.Verse = "";

            Bible_Chapter_ListBox.SelectedIndex = int.Parse(chapter) - 1;
            Bible_Verse_ListBox.SelectedIndex = int.Parse(verse) - 1;
        }

        // ======================================== 성경 출력 처리

        void Bible_Verse_ListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            BibleOutPut();
        }

        void BibleReserveListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            BibleOutPut();
        }

        void BibleOutputButton_Click(object sender, RoutedEventArgs e)
        {
            BibleOutPut();
        }

        void BibleOutPut()
        {
            if (module.ProgramOption.BibleFramePath == null)
                MessageBox.Show("성경 출력 틀ppt를 등록해주세요!", "ppt틀 등록되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (VM_BibleSelectData.Verse.CompareTo("") == 0)
                MessageBox.Show("출력할 성경구절을 선택해주세요!", "성경 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                return;//new module.ShowStarter().BibleShowStart(VM_BibleSelectData.Book + VM_BibleSelectData.Chapter + VM_BibleSelectData.Verse);
        }


        // ======================================== 성경 수정 처리

        void BibleModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM_BibleSelectData.Verse.CompareTo("") == 0)
                MessageBox.Show("수정할 성경구절을 선택해주세요!", "성경 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (SubWindow_BibleModify == null)
                {
                    SubWindow_BibleModify = new BibleModifyWindow(VM_BibleSelectData.Book + VM_BibleSelectData.Chapter + VM_BibleSelectData.Verse);
                    SubWindow_BibleModify.Owner = MainWindow.ProgramMainWindow;
                }
                else
                    SubWindow_BibleModify.setData(VM_BibleSelectData.Book + VM_BibleSelectData.Chapter + VM_BibleSelectData.Verse);
                SubWindow_BibleModify.ShowDialog();
            }
        }
    }
}
