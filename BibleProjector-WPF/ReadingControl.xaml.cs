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
    /// ReadingControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReadingControl : Window
    {
        static public ReadingControl ReadingControlAccess = null;

        private ViewModel.ReadingControlViewModel VM_ReadingControl;

        public ReadingControl(int ReadingNumber)
        {
            ReadingControlAccess = this;

            InitializeComponent();
            this.DataContext = VM_ReadingControl = new ViewModel.ReadingControlViewModel(ReadingNumber);
            setLayout();
        }

        public void ShowReading (int ReadingNumber)
        {
            VM_ReadingControl.showReading(ReadingNumber);
        }

        void setLayout()
        {
            if (module.LayoutInfo.Layout_ReadingControl.Width == -1)
                return;

            this.Width = module.LayoutInfo.Layout_ReadingControl.Width;
            this.Height = module.LayoutInfo.Layout_ReadingControl.Height;
            this.Left = module.LayoutInfo.Layout_ReadingControl.x;
            this.Top = module.LayoutInfo.Layout_ReadingControl.y;
        }

        // =================================================== 윈도우 레이아웃 변경 ======================================================

        public void ResetLayout()
        {
            this.Width = 533.438;
            this.Height = 336.375;
        }

        void changeSize(object sender, SizeChangedEventArgs e)
        {
            module.LayoutInfo.Layout_ReadingControl.Width = this.ActualWidth;
            module.LayoutInfo.Layout_ReadingControl.Height = this.ActualHeight;
            module.LayoutInfo.Layout_ReadingControl.x = this.Left;
            module.LayoutInfo.Layout_ReadingControl.y = this.Top;
        }

        void changeLocate(object sender, EventArgs e)
        {
            module.LayoutInfo.Layout_ReadingControl.Width = this.ActualWidth;
            module.LayoutInfo.Layout_ReadingControl.Height = this.ActualHeight;
            module.LayoutInfo.Layout_ReadingControl.x = this.Left;
            module.LayoutInfo.Layout_ReadingControl.y = this.Top;
        }

        // ================================================ 이벤트 처리 ================================================ 

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Right:
                    VM_ReadingControl.RunNextPage();
                    break;
                case Key.Down:
                case Key.Left:
                    VM_ReadingControl.RunPreviousPage();
                    break;
            }
        }

        void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_ReadingControl.RunPreviousPage();
        }

        void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_ReadingControl.RunNextPage();
        }

        void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ListBoxItem)(((ListBox)sender).ItemContainerGenerator.ContainerFromItem(((ListBox)sender).SelectedItem)))?.Focus();
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
                VM_ReadingControl.hideReading();
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}
