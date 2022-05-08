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

        public ExternPPTControl(string fileName,int StartSlide)
        {
            ExternPPTControlAccess = this;

            InitializeComponent();
            this.DataContext = VM_ExternPPTControl = new ViewModel.ExternPPTControlViewModel(fileName, StartSlide);
            
            setLayout();
        }

        public void ShowExternPPT (string fileName, int StartSlide)
        {
            VM_ExternPPTControl.ShowExternPPT(fileName, StartSlide);
        }

        public void RefreshExternPPT(string fileName)
        {
            VM_ExternPPTControl.RefreshExternPPT(fileName);
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
            this.Width = 533.438;
            this.Height = 336.375;
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

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
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
            }
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
