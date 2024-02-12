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
        }

        // ========== BindingProperties ==========

        System.Reflection.PropertyInfo _CSearchStartProperty = null;
        System.Reflection.PropertyInfo CSearchStartProperty { 
            get
            {
                return _CSearchStartProperty
                    ?? this.DataContext.GetType().GetProperty("CSearchStart")
                    ?? throw new Exception("Binding Error");
            }
        }

        System.Reflection.PropertyInfo _CPopupHideProperty = null;
        System.Reflection.PropertyInfo CPopupHideProperty {
            get
            {
                return _CPopupHideProperty
                    ?? this.DataContext.GetType().GetProperty("CPopupHide")
                    ?? throw new Exception("Binding Error");
            }
        }

        System.Reflection.PropertyInfo _CLastestResultShowProperty = null;
        System.Reflection.PropertyInfo CLastestResultShowProperty
        {
            get
            {
                return _CLastestResultShowProperty
                    ?? this.DataContext.GetType().GetProperty("CLastestResultShow")
                    ?? throw new Exception("Binding Error");
            }
        }

        // ========== EventHander ==========

        void EH_TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (e.Key == Key.Enter)
                ((ICommand)CSearchStartProperty.GetValue(this.DataContext)).Execute(null);
            if (e.Key == Key.Escape)
                ((ICommand)CPopupHideProperty.GetValue(this.DataContext)).Execute(null);
            else if (e.Key == Key.Down)
            {
                if (ResultListBox.Items.Count != 0)
                    ((ICommand)CLastestResultShowProperty.GetValue(this.DataContext)).Execute(null);
                ResultListBox.Focus();
            }
        }

        void EH_ListBoxKeyUp(object sender, KeyEventArgs e)
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
    }
}
