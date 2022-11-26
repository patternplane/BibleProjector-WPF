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

        public void DeletingCheckAndClose(string DeletingFrameFileName)
        {
            if (VM_SongControl.isSameFrame(DeletingFrameFileName))
                this.Close();
        }

        public SongControl(string[][][] songData,string path, bool isHymn)
        {
            SongControlAccess = this;

            InitializeComponent();
            this.DataContext = VM_SongControl = new ViewModel.SongControlViewModel(songData, System.IO.Path.GetFileName(path), isHymn);
            setLayout();
        }

        public void ShowSong (string[][][] songData, string path, bool isHymn)
        {
            VM_SongControl.showSong(songData, System.IO.Path.GetFileName(path), isHymn);
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

        void Window_Activated(object sender, EventArgs e)
        {
            VM_SongControl.NumInput_Remove();
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9
                || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                VM_SongControl.NumInput(KeyToNum(e.Key));
                return;
            }

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
                case Key.Enter:
                    VM_SongControl.NumInput_Enter();
                    break;
            }

            VM_SongControl.NumInput_Remove();
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

        void setSlideTopMost(object sender, RoutedEventArgs e)
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
