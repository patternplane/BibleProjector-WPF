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
    /// SearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchView : UserControl
    {
        public SearchView()
        {
            InitializeComponent();

            Binding b = new Binding("CItemSelected");
            this.SetBinding(ItemSelectedCommandProperty, b);

            // 테스팅용
            //b = new Binding("CItemClick");
            //this.SetBinding(CItemClickProperty, b);
        }

        // ========== BindingProperties ==========

        System.Reflection.PropertyInfo _CSearchStartProperty = null;
        System.Reflection.PropertyInfo CSearchStartProperty { 
            get
            {
                if (_CSearchStartProperty == null)
                    _CSearchStartProperty = this.DataContext.GetType().GetProperty("CSearchStart")
                        ?? throw new Exception("Binding Error");

                return _CSearchStartProperty;
            }
        }

        System.Reflection.PropertyInfo _CPopupHideProperty = null;
        System.Reflection.PropertyInfo CPopupHideProperty {
            get
            {
                if (_CPopupHideProperty == null)
                    _CPopupHideProperty = this.DataContext.GetType().GetProperty("CPopupHide")
                        ?? throw new Exception("Binding Error");

                return _CPopupHideProperty;
            }
        }

        System.Reflection.PropertyInfo _CLastestResultShowProperty = null;
        System.Reflection.PropertyInfo CLastestResultShowProperty
        {
            get
            {
                if (_CLastestResultShowProperty == null)
                    _CLastestResultShowProperty = this.DataContext.GetType().GetProperty("CLastestResultShow")
                        ?? throw new Exception("Binding Error");

                return _CLastestResultShowProperty;
            }
        }

        public static readonly DependencyProperty ItemSelectedCommandProperty =
        DependencyProperty.Register(
            name: "ItemSelectedCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(SearchView));

        public ICommand ItemSelectedCommand
        {
            get => (ICommand)GetValue(ItemSelectedCommandProperty);
            set => SetValue(ItemSelectedCommandProperty, value);
        }

        // ========== EventHander ==========

        private void EHS_NewResult()
        {
            ((ICommand)CSearchStartProperty.GetValue(this.DataContext)).Execute(null);
            if (ResultListBox.Items.Count > 0)
            {
                ResultListBox.Focus();
                try
                {
                    ((ListBoxItem)ResultListBox.ItemContainerGenerator.ContainerFromIndex(0)).Focus();
                }
                catch { }
            }
        }

        void EH_TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (e.Key == Key.Enter)
                EHS_NewResult();
            else if (e.Key == Key.Escape)
                ((ICommand)CPopupHideProperty.GetValue(this.DataContext)).Execute(null);
            else if (e.Key == Key.Down)
            {
                if (ResultListBox.Items.Count != 0)
                {
                    ResultListBox.Focus();
                    ((ICommand)CLastestResultShowProperty.GetValue(this.DataContext)).Execute(null);
                    ((ListBoxItem)ResultListBox.ItemContainerGenerator.ContainerFromIndex(
                        ResultListBox.SelectedIndex < 0 ? 0 : ResultListBox.SelectedIndex
                        )).Focus();
                }
            }
        }

        int currentSelectedIdx = -1;
        void EH_ListBoxKeyDown(object sender, KeyEventArgs e)
        {
            currentSelectedIdx = ResultListBox.SelectedIndex;
        }

        void EH_ListBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape
                || e.Key == Key.Enter)
            {
                ((ICommand)CPopupHideProperty.GetValue(this.DataContext)).Execute(null);
                SearchTextBox.Focus();
            }
            else if (e.Key == Key.Up
                && currentSelectedIdx == 0)
                SearchTextBox.Focus();
        }

        void EH_SearchItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count >= 1)
                ItemSelectedCommand?.Execute(e.AddedItems[0]);
        }

        private void ListBoxItem_MouseClick(object sender, MouseButtonEventArgs e)
        {
            ((ICommand)CPopupHideProperty.GetValue(this.DataContext)).Execute(null);
            SearchTextBox.Focus();
        }

        private void EH_SearchButtonClick(object sender, MouseButtonEventArgs e)
        {
            EHS_NewResult();
        }

        // ======================= 자동 텍스트 전체선택 =======================

        private void EH_TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
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
