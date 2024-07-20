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
    }
}
