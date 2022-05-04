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
    /// BibleControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SongControl : Window
    {
        static public SongControl SongControlAccess = null;

        private ViewModel.SongControlViewModel VM_SongControl;

        public SongControl(string[][][] songData,string path)
        {
            SongControlAccess = this;

            InitializeComponent();
            this.DataContext = VM_SongControl = new ViewModel.SongControlViewModel(songData, System.IO.Path.GetFileName(path));
            setLayout();
        }

        public void ShowSong (string[][][] songData, string path)
        {
            VM_SongControl.showSong(songData, System.IO.Path.GetFileName(path));
        }

        void setLayout()
        {
            if (module.LayoutInfo.Layout_SongControl.Width == -1)
                return;

            this.Width = module.LayoutInfo.Layout_SongControl.Width;
            this.Height = module.LayoutInfo.Layout_SongControl.Height;
            this.Left = module.LayoutInfo.Layout_SongControl.x;
            this.Top = module.LayoutInfo.Layout_SongControl.y;
        }

        // =================================================== 윈도우 레이아웃 변경 ======================================================

        public void ResetLayout()
        {
            this.Width = 533.438;
            this.Height = 336.375;
        }

        void changeSize(object sender, SizeChangedEventArgs e)
        {
            module.LayoutInfo.Layout_SongControl.Width = this.ActualWidth;
            module.LayoutInfo.Layout_SongControl.Height = this.ActualHeight;
            module.LayoutInfo.Layout_SongControl.x = this.Left;
            module.LayoutInfo.Layout_SongControl.y = this.Top;
        }

        void changeLocate(object sender, EventArgs e)
        {
            module.LayoutInfo.Layout_SongControl.Width = this.ActualWidth;
            module.LayoutInfo.Layout_SongControl.Height = this.ActualHeight;
            module.LayoutInfo.Layout_SongControl.x = this.Left;
            module.LayoutInfo.Layout_SongControl.y = this.Top;
        }

        // ================================================ 이벤트 처리 ================================================ 

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Right:
                    VM_SongControl.RunNextPage();
                    break;
                case Key.Down:
                case Key.Left:
                    VM_SongControl.RunPreviousPage();
                    break;
            }
        }

        void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_SongControl.RunPreviousPage();
        }

        void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_SongControl.RunNextPage();
        }
        void PageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ListBoxItem)(((ListBox)sender).ItemContainerGenerator.ContainerFromItem(((ListBox)sender).SelectedItem)))?.Focus();
        }

        // ========================================== 윈도우 최상위 ======================================

        void setSlideTopMost(object sender, EventArgs e)
        {
            VM_SongControl.RunTopMost();
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
                VM_SongControl.hideSong();
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
    }
}
