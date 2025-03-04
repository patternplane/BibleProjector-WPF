﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Threading;

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
            SetBinding(CSetTopMostProperty, new Binding("CSetDisplayTopMost"));

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

        public static readonly DependencyProperty CSetTopMostProperty =
            DependencyProperty.Register(
                name: "CSetTopMost",
                propertyType: typeof(ICommand),
                ownerType: typeof(ShowControler));

        public ICommand CSetTopMost
        {
            get => (ICommand)GetValue(CSetTopMostProperty);
            set => SetValue(CSetTopMostProperty, value);
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
                if (!((ShowControler)d).IsKeyboardFocusWithin)
                    ((ShowControler)d).Focus();
        }

        // ========== EventHandler ==========

        void EH_PreviousButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                getCGoPreviousPageProperty(this.DataContext).Execute(false);
            else if (e.ChangedButton == MouseButton.Right)
                getCGoPreviousPageProperty(this.DataContext).Execute(true);
        }

        void EH_NextButtonClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                getCGoNextPageProperty(this.DataContext).Execute(false);
            else if (e.ChangedButton == MouseButton.Right)
                getCGoNextPageProperty(this.DataContext).Execute(true);
        }

        private void EH_DisplayOffButtonClick(object sender, RoutedEventArgs e)
        {
            getCDisplayOnOffProperty(this.DataContext).Execute(!((ToggleButton)sender).IsChecked);
        }

        private void EH_TextShowButtonClick(object sender, RoutedEventArgs e)
        {
            getCTextShowHideProperty(this.DataContext).Execute(!((ToggleButton)sender).IsChecked);
        }

        private void EH_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CSetTopMost.Execute(null);
        }

        private void EH_ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta >= 0)
                getCGoPreviousPageProperty(this.DataContext).Execute(false);
            else
                getCGoNextPageProperty(this.DataContext).Execute(false);
        }

        // =============================== focusing and key ignore =============================== 

        private void EH_focusIn(object sender, RoutedEventArgs e)
        {
            if (!hasFocus)
                hasFocus = true;
        }

        private void EH_mouseFocus(object sender, MouseButtonEventArgs e)
        {
            if (!hasFocus)
                hasFocus = true;
        }

        private void EH_focusOut(object sender, RoutedEventArgs e)
        {
            if (this.IsKeyboardFocusWithin)
                return;

            if (hasFocus)
                hasFocus = false;
        }

        private void EH_SelectedItemFocuser(object sender, SelectionChangedEventArgs e)
        {
            ((ListBoxItem)((ListBox)sender).ItemContainerGenerator.ContainerFromItem(((ListBox)sender).SelectedItem))?.Focus();
        }

        private void EH_OffUnusedKeyInput(object sender, KeyEventArgs e)
        {
            // <이 코드가 맡는 역할 명세>
            //
            // ● 역할
            //   이 코드는 화살표 키로 이동시 2중 이동하는 문제를 다룹니다.
            //
            // ● 원인 분석
            //   1. 키 조작에 대한 처리는 메인에서 진행함.
            //   2. 키 입력 이벤트는 [상위 뷰 -> 하위 뷰] 순서로 진행됨.
            //   3. 만약 이 뷰에서 키 입력을 받아들이면,
            //        1) 메인 뷰의 키 입력으로 이미 페이지를 이동한 상태에서
            //        2) ListBox도 키 입력을 받아 항목을 이동하면서
            //      2중 이동이 발생함
            //
            // ● 처리
            //   이 뷰의 화살표 키 입력은 무시함으로써 ListBox의 화살표 키 인식을 막도록 함.

            if (e.Key == Key.Left
                || e.Key == Key.Down
                || e.Key == Key.Right
                || e.Key == Key.Up)
                e.Handled = true;
        }
    }
}
