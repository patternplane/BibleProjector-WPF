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
using System.Windows.Shapes;

namespace BibleProjector_WPF
{
    /// <summary>
    /// ExternPPTControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExternPPTControl : Window
    {
        static public ExternPPTControl ExternPPTControlAccess = null;

        private ViewModel.ExternPPTControlViewModel VM_ExternPPTControl;

        public void CheckDeletedPPTAndClose(string FileName)
        {
            if (VM_ExternPPTControl.isSameFileName(FileName))
                this.Close();
        }

        public ExternPPTControl()
        {
            ExternPPTControlAccess = this;

            InitializeComponent();
            this.DataContext = VM_ExternPPTControl = new ViewModel.ExternPPTControlViewModel();
            
            setLayout();
        }

        void setLayout()
        {
            if (module.LayoutInfo.Layout_ExternPPTControl.Width == -1)
                return;

            this.Width = module.LayoutInfo.Layout_ExternPPTControl.Width;
            this.Height = module.LayoutInfo.Layout_ExternPPTControl.Height;
            this.Left = module.LayoutInfo.Layout_ExternPPTControl.x;
            this.Top = module.LayoutInfo.Layout_ExternPPTControl.y;
        }

        // =================================================== 윈도우 레이아웃 변경 ======================================================

        public void ResetLayout()
        {
            this.Width = 570;
            this.Height = 340;
        }

        void changeSize(object sender, SizeChangedEventArgs e)
        {
            module.LayoutInfo.Layout_ExternPPTControl.Width = this.ActualWidth;
            module.LayoutInfo.Layout_ExternPPTControl.Height = this.ActualHeight;
            module.LayoutInfo.Layout_ExternPPTControl.x = this.Left;
            module.LayoutInfo.Layout_ExternPPTControl.y = this.Top;
        }

        void changeLocate(object sender, EventArgs e)
        {
            module.LayoutInfo.Layout_ExternPPTControl.Width = this.ActualWidth;
            module.LayoutInfo.Layout_ExternPPTControl.Height = this.ActualHeight;
            module.LayoutInfo.Layout_ExternPPTControl.x = this.Left;
            module.LayoutInfo.Layout_ExternPPTControl.y = this.Top;
        }

        // ================================================ 이벤트 처리 ================================================ 

        void Window_Activated(object sender, EventArgs e)
        {
            VM_ExternPPTControl.NumInput_Remove();
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9
                || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                VM_ExternPPTControl.NumInput(KeyToNum(e.Key));
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                case Key.Right:
                    VM_ExternPPTControl.RunNextPage();
                    break;
                case Key.Down:
                case Key.Left:
                    VM_ExternPPTControl.RunPreviousPage();
                    break;
                case Key.Enter:
                    VM_ExternPPTControl.NumInput_Enter();
                    break;
            }

            VM_ExternPPTControl.NumInput_Remove();
        }
        int KeyToNum(Key key)
        {
            if (key >= Key.D0 && key <= Key.D9)
                return (key - Key.D0);
            else if (key >= Key.NumPad0 && key <= Key.NumPad9)
                return (key - Key.NumPad0);
            else
                return -1;
        }

        void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_ExternPPTControl.RunPreviousPage();
        }

        void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_ExternPPTControl.RunNextPage();
        }

        void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ListBoxItem)(((ListBox)sender).ItemContainerGenerator.ContainerFromItem(((ListBox)sender).SelectedItem)))?.Focus();
        }

        // ========================================== 윈도우 최상위 ======================================

        void setSlideTopMost(object sender, RoutedEventArgs e)
        {
            VM_ExternPPTControl.RunTopMost();
        }

        // ========================================== 윈도우 처리 =====================================

        private bool AllowClose = false;

        public void ForceClose()
        {
            AllowClose = true;
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (AllowClose)
                e.Cancel = false;
            else
            {
                this.Hide();
                VM_ExternPPTControl.HideExternPPT();
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}
