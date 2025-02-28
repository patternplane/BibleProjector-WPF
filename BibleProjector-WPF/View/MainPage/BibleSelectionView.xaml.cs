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

namespace BibleProjector_WPF.View.MainPage
{
    /// <summary>
    /// BibleSelectionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BibleSelectionView : UserControl
    {
        public BibleSelectionView()
        {
            InitializeComponent();

            SetBinding(CBookSelectionProperty, new Binding("CBookSelection"));
            SetBinding(CChapterSelectionProperty, new Binding("CChapterSelection")); 
            SetBinding(CVerseSelectionProperty, new Binding("CVerseSelection"));
            SetBinding(CShowBibleProperty, new Binding("CShowBible"));
            SetBinding(CReserveBibleProperty, new Binding("CReserveBible"));

            SetBinding(CSearchBibleProperty, new Binding("CSearchBible"));
            SetBinding(CMoveToPrevPageProperty, new Binding("CMoveToPrevPage"));
            SetBinding(CMoveToNextPageProperty, new Binding("CMoveToNextPage"));

            SetBinding(SetBookSelectionProperty, new Binding("BookSetter") { Mode = BindingMode.TwoWay });
            SetBinding(SetChapterSelectionProperty, new Binding("ChapterSetter") { Mode = BindingMode.TwoWay });
            SetBinding(SetVerseSelectionProperty, new Binding("VerseSetter") { Mode = BindingMode.TwoWay });
        }

        // ========== BindingProperties ==========

        public static readonly DependencyProperty CBookSelectionProperty =
            DependencyProperty.Register(
                name: "CBookSelection",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CBookSelection
        {
            get => (ICommand)GetValue(CBookSelectionProperty);
            set => SetValue(CBookSelectionProperty, value);
        }

        public static readonly DependencyProperty CChapterSelectionProperty =
            DependencyProperty.Register(
                name: "CChapterSelection",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CChapterSelection
        {
            get => (ICommand)GetValue(CChapterSelectionProperty);
            set => SetValue(CChapterSelectionProperty, value);
        }

        public static readonly DependencyProperty CVerseSelectionProperty =
            DependencyProperty.Register(
                name: "CVerseSelection",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CVerseSelection
        {
            get => (ICommand)GetValue(CVerseSelectionProperty);
            set => SetValue(CVerseSelectionProperty, value);
        }

        public static readonly DependencyProperty CShowBibleProperty =
            DependencyProperty.Register(
                name: "CShowBible",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CShowBible
        {
            get => (ICommand)GetValue(CShowBibleProperty);
            set => SetValue(CShowBibleProperty, value);
        }

        public static readonly DependencyProperty CReserveBibleProperty =
            DependencyProperty.Register(
                name: "CReserveBible",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CReserveBible
        {
            get => (ICommand)GetValue(CReserveBibleProperty);
            set => SetValue(CReserveBibleProperty, value);
        }

        public static readonly DependencyProperty CSearchBibleProperty =
            DependencyProperty.Register(
                name: "CSearchBible",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CSearchBible
        {
            get => (ICommand)GetValue(CSearchBibleProperty);
            set => SetValue(CSearchBibleProperty, value);
        }

        public static readonly DependencyProperty CMoveToPrevPageProperty =
            DependencyProperty.Register(
                name: "CMoveToPrevPage",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CMoveToPrevPage
        {
            get => (ICommand)GetValue(CMoveToPrevPageProperty);
            set => SetValue(CMoveToPrevPageProperty, value);
        }

        public static readonly DependencyProperty CMoveToNextPageProperty =
            DependencyProperty.Register(
                name: "CMoveToNextPage",
                propertyType: typeof(ICommand),
                ownerType: typeof(BibleSelectionView));

        public ICommand CMoveToNextPage
        {
            get => (ICommand)GetValue(CMoveToNextPageProperty);
            set => SetValue(CMoveToNextPageProperty, value);
        }

        public static readonly DependencyProperty SetBookSelectionProperty =
            DependencyProperty.Register(
                name: "SetBookSelection",
                propertyType: typeof(int),
                ownerType: typeof(BibleSelectionView),
                new PropertyMetadata(EH_SetBookSelection));

        public int SetBookSelection
        {
            get => (int)GetValue(SetBookSelectionProperty);
            set => SetValue(SetBookSelectionProperty, value);
        }

        public static readonly DependencyProperty SetChapterSelectionProperty =
            DependencyProperty.Register(
                name: "SetChapterSelection",
                propertyType: typeof(int),
                ownerType: typeof(BibleSelectionView),
                new PropertyMetadata(EH_SetChapterSelection));

        public int SetChapterSelection
        {
            get => (int)GetValue(SetChapterSelectionProperty);
            set => SetValue(SetChapterSelectionProperty, value);
        }

        public static readonly DependencyProperty SetVerseSelectionProperty =
            DependencyProperty.Register(
                name: "SetVerseSelection",
                propertyType: typeof(int),
                ownerType: typeof(BibleSelectionView),
                new PropertyMetadata(EH_SetVerseSelection));

        public int SetVerseSelection
        {
            get => (int)GetValue(SetVerseSelectionProperty);
            set => SetValue(SetVerseSelectionProperty, value);
        }

        // ========== Event Handling ==========

        private bool allowResponse = true;

        private void EH_BookSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // New Selecting Case && Allow Response of UI Change
            if (e.AddedItems.Count != 0 && allowResponse)
            {
                if (sender == UE_OldBookList)
                {
                    CBookSelection.Execute(UE_OldBookList.SelectedIndex + 1);
                    UE_NewBookList.SelectedIndex = -1;
                }
                else if (sender == UE_NewBookList)
                {
                    CBookSelection.Execute(UE_NewBookList.SelectedIndex + 1 + UE_OldBookList.Items.Count);
                    UE_OldBookList.SelectedIndex = -1;
                }
            }
        }

        private void EH_ChapterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // New Selecting Case && Allow Response of UI Change
            if (e.AddedItems.Count != 0 && allowResponse)
                CChapterSelection.Execute(((ListBox)sender).SelectedIndex + 1);
        }

        private void EH_VerseSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // New Selecting Case && Allow Response of UI Change
            if (e.AddedItems.Count != 0 && allowResponse)
                CVerseSelection.Execute(((ListBox)sender).SelectedIndex + 1);
        }

        private static void EH_SetBookSelection(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == 0)
                return;

            BibleSelectionView me = (BibleSelectionView)d;
            me.allowResponse = false;

            int book = (int)e.NewValue;
            if (book > 0 && book <= 39)
            {
                me.UE_NewBookList.SelectedIndex = -1;
                me.UE_OldBookList.SelectedIndex = book - 1;
                me.UE_OldBookList.ScrollIntoView(me.UE_OldBookList.SelectedItem);
                me.UE_OldBookTab.IsSelected = true;
            }
            else if (book > 39 && book <= 66)
            {
                me.UE_OldBookList.SelectedIndex = -1;
                me.UE_NewBookList.SelectedIndex = book - 1 - 39;
                me.UE_NewBookList.ScrollIntoView(me.UE_NewBookList.SelectedItem);
                me.UE_NewBookTab.IsSelected = true;
            }
            else
            {
                me.UE_OldBookList.SelectedIndex = -1;
                me.UE_NewBookList.SelectedIndex = -1;
            }

            me.allowResponse = true;

            d.SetValue(SetBookSelectionProperty, 0);
        }

        private static void EH_SetChapterSelection(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == 0)
                return;

            BibleSelectionView me = (BibleSelectionView)d;
            me.allowResponse = false;

            int chapter = (int)e.NewValue;
            if (chapter >= 1 && chapter <= me.UE_ChapterList.Items.Count)
            {
                me.UE_ChapterList.SelectedIndex = chapter - 1;
                me.UE_ChapterList.ScrollIntoView(me.UE_ChapterList.SelectedItem);
            }
            else
                me.UE_ChapterList.SelectedIndex = -1;

            me.allowResponse = true;

            d.SetValue(SetChapterSelectionProperty, 0);
        }

        private static void EH_SetVerseSelection(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue == 0)
                return;

            BibleSelectionView me = (BibleSelectionView)d;
            me.allowResponse = false;

            int verse = (int)e.NewValue;
            if (verse >= 1 && verse <= me.UE_VerseList.Items.Count)
            {
                me.UE_VerseList.SelectedIndex = verse - 1;
                me.UE_VerseList.ScrollIntoView(me.UE_VerseList.SelectedItem);
            }
            else
                me.UE_VerseList.SelectedIndex = -1;

            me.allowResponse = true;

            d.SetValue(SetVerseSelectionProperty, 0);
        }

        private void EH_BibleOpenDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CShowBible.Execute(null);
        }

        private void EH_ShowBibleButtonClick(object sender, RoutedEventArgs e)
        {
            CShowBible.Execute(null);
        }

        private void EH_ReserveBibleButtonClick(object sender, RoutedEventArgs e)
        {
            CReserveBible.Execute(null);
        }

        // ======================= 검색 결과 팝업 표시/조작 =======================

        private void EH_DropDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResultListBox.Items.Count != 0)
                openPopupAndDisplaySelection();
        }

        private void EH_ListBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && ResultListBox.SelectedIndex <= 0)
            {
                closePopup();
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
                closePopup(true);
            if (e.Key == Key.Escape
                || e.Key == Key.Back)
                closePopup();

            if (e.Key == Key.Left)
            {
                CMoveToPrevPage.Execute(null);
                e.Handled = true;
            }
            if (e.Key == Key.Right)
            {
                CMoveToNextPage.Execute(null);
                e.Handled = true;
            }
        }

        private void EH_ListBoxItem_MouseClick(object sender, MouseButtonEventArgs e)
        {
            closePopup(true);
        }

        private void EH_TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (e.Key == Key.Enter)
                searchBible();
            else if (e.Key == Key.Escape)
                closePopup();
        }

        private void EH_TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            // KeyDown 시점에서 Popup으로 포커싱이 옮겨가면
            // KeyUp이벤트가 Popup에서 발생하게 되어
            // 의도하지 않은 동작이 발생할 수 있음.
            // 이에 따라, KeyUp 시점에서 Popup으로 포커싱을 이동하도록 함.
            if (e.Key == Key.Down && ResultListBox.Items.Count != 0)
                openPopupAndDisplaySelection();
        }

        private void EH_SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchBible();
        }

        private void openPopupAndDisplaySelection()
        {
            if (ResultListBox.Items.Count == 0)
                return;

            SearchResultPopup.IsOpen = true;

            if (ResultListBox.SelectedIndex >= 0)
                ((ListBoxItem)ResultListBox.ItemContainerGenerator.ContainerFromIndex(ResultListBox.SelectedIndex)).Focus();
        }

        private void EH_SearchResultPopup_Opened(object sender, EventArgs e)
        {
            ResultListBox.Focus();
            if (ResultListBox.SelectedIndex < 0
                && ResultListBox.Items.Count > 0)
                ResultListBox.ScrollIntoView(ResultListBox.Items[0]);
        }

        private void closePopup(bool forceRefresh = false)
        {
            SearchResultPopup.IsOpen = false;
            SearchTextBox.Focus();
            if (forceRefresh)
                ResultListBox.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource();
        }

        private void searchBible()
        {
            CSearchBible.Execute(null);
            if (SearchTextBox.Text == null || SearchTextBox.Text.Length < 2)
                MessageBox.Show("검색 문구를 2자 이상 입력하세요!", "검색 문구가 짧아요", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // ======================= 검색 결과 페이지 이동 =======================

        private void EH_PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            CMoveToPrevPage.Execute(null);
        }

        private void EH_NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CMoveToNextPage.Execute(null);
        }

        private void EH_PageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
            if (e.Key == Key.Up)
                closePopup();
        }

        private void EH_PageTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
                ResultListBox.Focus();
        }

        // ======================= 페이지 이동 텍스트 힌트 표시 =======================

        private void EH_PageTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PageMoveTextBoxHint.Visibility = Visibility.Hidden;
        }

        private void EH_PageTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == null || ((TextBox)sender).Text.Length == 0)
                PageMoveTextBoxHint.Visibility = Visibility.Visible;
            else
                PageMoveTextBoxHint.Visibility = Visibility.Hidden;
        }

        // ======================= 검색 텍스트 힌트 표시 =======================

        private void hideHint()
        {
            SearchTextBoxHint.Visibility = Visibility.Hidden;
        }

        private void EH_TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == null || SearchTextBox.Text.Length == 0)
                SearchTextBoxHint.Visibility = Visibility.Visible;
            else
                SearchTextBoxHint.Visibility = Visibility.Hidden;
        }

        // ======================= 자동 텍스트 전체선택 =======================

        private void EH_TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            hideHint();
            SearchTextBox.SelectAll();
        }

        private void EH_TextBoxClick(object sender, MouseButtonEventArgs e)
        {
            if (!SearchTextBox.IsFocused)
            {
                SearchTextBox.Focus();
                e.Handled = true;
            }
        }

        private void EH_TextBoxDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SearchTextBox.SelectAll();
        }
    }
}
