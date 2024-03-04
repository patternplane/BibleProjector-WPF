using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// ShowControler.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ShowControler : UserControl
    {
        public ShowControler()
        {
            InitializeComponent();
        }

        // 언제 숫자입력값을 다시 초기화해야 할지 곰곰히 생각해봐야 할 듯 합니다.
        /*void Window_Activated(object sender, EventArgs e)
        {
            VM_BibleControl.NumInput_Remove();
        }*//*

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9
                || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                VM_BibleControl.NumInput(KeyToNum(e.Key));
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                case Key.Right:
                    VM_BibleControl.RunNextPage();
                    break;
                case Key.Down:
                case Key.Left:
                    VM_BibleControl.RunPreviousPage();
                    break;
                case Key.Enter:
                    VM_BibleControl.NumInput_Enter();
                    break;
            }

            VM_BibleControl.NumInput_Remove();
        }
        int KeyToNum(Key key)
        {
            if (key >= Key.D0 && key <= Key.D9)
                return (key - Key.D0);
            else if (key >= Key.NumPad0 && key <= Key.NumPad9)
                return (key - Key.NumPad0);
            else
                return -1;
        }*/

        // ========== BindingProperties ==========
        
        System.Reflection.PropertyInfo _CDisplayOnOffProperty = null;
        ICommand getCDisplayOnOffProperty(object obj)
        {
            if (_CDisplayOnOffProperty == null)
                _CDisplayOnOffProperty = obj.GetType().GetProperty("CDisplayOnOff")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CDisplayOnOffProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CTextShowHideProperty = null;
        ICommand getCTextShowHideProperty(object obj)
        {
            if (_CTextShowHideProperty == null)
                _CTextShowHideProperty = obj.GetType().GetProperty("CTextShowHide")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CTextShowHideProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CGoNextPageProperty = null;
        ICommand getCGoNextPageProperty(object obj)
        {
            if (_CGoNextPageProperty == null)
                _CGoNextPageProperty = obj.GetType().GetProperty("CGoNextPage")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CGoNextPageProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CGoPreviousPageProperty = null;
        ICommand getCGoPreviousPageProperty(object obj)
        {
            if (_CGoPreviousPageProperty == null)
                _CGoPreviousPageProperty = obj.GetType().GetProperty("CGoPreviousPage")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CGoPreviousPageProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CSetDisplayTopMostProperty = null;
        ICommand getCSetDisplayTopMostProperty(object obj)
        {
            if (_CSetDisplayTopMostProperty == null)
                _CSetDisplayTopMostProperty = obj.GetType().GetProperty("CSetDisplayTopMost")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CSetDisplayTopMostProperty.GetValue(obj);
        }

        // ========== EventHandler ==========

        void EH_GoPrevious(object sender, RoutedEventArgs e)
        {
            getCGoPreviousPageProperty(this.DataContext).Execute(null);
        }

        void EH_GoNext(object sender, RoutedEventArgs e)
        {
            getCGoNextPageProperty(this.DataContext).Execute(null);
        }

        void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox ControlListBox = (ListBox)sender;
            ListBoxItem SelectedItem = (ListBoxItem)ControlListBox.ItemContainerGenerator.ContainerFromItem(ControlListBox.SelectedItem);
            SelectedItem?.Focus();
        }

        private void EH_DisplayOffButtonClick(object sender, RoutedEventArgs e)
        {
            getCDisplayOnOffProperty(this.DataContext).Execute(!((ToggleButton)sender).IsChecked);
        }

        private void EH_TextShowButtonClick(object sender, RoutedEventArgs e)
        {
            getCTextShowHideProperty(this.DataContext).Execute(!((ToggleButton)sender).IsChecked);
        }
    }
}
