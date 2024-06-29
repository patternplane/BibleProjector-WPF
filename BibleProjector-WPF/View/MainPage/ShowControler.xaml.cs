﻿using System;
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

            SetBinding(hasFocusProperty, new Binding("hasFocus") { Mode = BindingMode.TwoWay });

            hasFocus = false;
        }

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

        public static readonly DependencyProperty hasFocusProperty =
            DependencyProperty.Register(
                name: "hasFocus",
                propertyType: typeof(bool),
                ownerType: typeof(ShowControler),
                new PropertyMetadata(hasFocusChanged));

        public bool hasFocus
        {
            get => (bool)GetValue(hasFocusProperty);
            set => SetValue(hasFocusProperty, value);
        }

        static void hasFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       {
            if ((bool)e.NewValue)
            {
                if (!((ShowControler)d).IsKeyboardFocusWithin)
                    ((ShowControler)d).Focus();
                ((ShowControler)d).focusBorder.Visibility = Visibility.Hidden;
            }
            else
                ((ShowControler)d).focusBorder.Visibility = Visibility.Visible;
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

        private void EH_DisplayOffButtonClick(object sender, RoutedEventArgs e)
        {
            getCDisplayOnOffProperty(this.DataContext).Execute(!((ToggleButton)sender).IsChecked);
        }

        private void EH_TextShowButtonClick(object sender, RoutedEventArgs e)
        {
            getCTextShowHideProperty(this.DataContext).Execute(!((ToggleButton)sender).IsChecked);
        }

        // =============================== focusing and key ignore =============================== 

        private void EH_focusIn(object sender, RoutedEventArgs e)
        {
            hasFocus = true;
        }

        private void EH_mouseFocus(object sender, MouseButtonEventArgs e)
        {
            hasFocus = true;
        }

        private void EH_focusOut(object sender, RoutedEventArgs e)
        {
            hasFocus = false;
        }

        private void EH_SelectedItemFocuser(object sender, SelectionChangedEventArgs e)
        {
            ((ListBoxItem)((ListBox)sender).ItemContainerGenerator.ContainerFromItem(((ListBox)sender).SelectedItem))?.Focus();
        }

        private void EH_OffUnusedKeyInput(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
