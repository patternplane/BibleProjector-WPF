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
        private ViewModel.SongControlViewModel VM_SongControl;

        public SongControl(string[][][] songData,string path)
        {
            InitializeComponent();
            this.DataContext = VM_SongControl = new ViewModel.SongControlViewModel(songData, System.IO.Path.GetFileName(path));
        }

        public void ShowSong (string[][][] songData, string path)
        {
            VM_SongControl.showSong(songData, System.IO.Path.GetFileName(path));
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
