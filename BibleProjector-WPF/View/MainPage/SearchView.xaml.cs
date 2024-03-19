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
            // 테스팅용
            Binding b = new Binding("CItemClick");
            this.SetBinding(CItemClickProperty, b);
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

        // ========== EventHander ==========

        void EH_TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (e.Key == Key.Enter)
                ((ICommand)CSearchStartProperty.GetValue(this.DataContext)).Execute(null);
            else if (e.Key == Key.Escape)
                ((ICommand)CPopupHideProperty.GetValue(this.DataContext)).Execute(null);
            else if (e.Key == Key.Down)
            {
                if (ResultListBox.Items.Count != 0)
                {
                    ((ICommand)CLastestResultShowProperty.GetValue(this.DataContext)).Execute(null);
                    ResultListBox.Focus();
                    ((ListBoxItem)ResultListBox.ItemContainerGenerator.ContainerFromIndex(0)).Focus();
                }
            }
        }

        void EH_ListBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((ICommand)CPopupHideProperty.GetValue(this.DataContext)).Execute(null);
                SearchTextBox.Focus();
            }
            else if (e.Key == Key.Up
                && ResultListBox.SelectedIndex == 0)
                SearchTextBox.Focus();
        }

        public static readonly DependencyProperty CItemClickProperty =
        DependencyProperty.Register(
            name: "CItemClick",
            propertyType: typeof(ICommand),
            ownerType: typeof(SearchView));

        public ICommand CItemClick
        {
            get => (ICommand)GetValue(CItemClickProperty);
            set => SetValue(CItemClickProperty, value);
        }
        private void ListBoxItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.MainPage.VMSearchResult item = (ViewModel.MainPage.VMSearchResult)((ListBoxItem)sender).DataContext;
            
            Console.WriteLine(item.DisplayTitle);
            // 테스팅용
            CItemClick.Execute(item.getData());
        }
    }
}
