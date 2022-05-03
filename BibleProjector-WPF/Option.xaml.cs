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

namespace BibleProjector_WPF
{
    /// <summary>
    /// Option.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Option : UserControl
    {

        // ViewModel
        ViewModel.OptionViewModel VM_Option;

        public Option()
        {
            InitializeComponent();
            MainOptionGrid.DataContext = VM_Option = new ViewModel.OptionViewModel();
        }

        // ========================================= 틀 파일 등록 ==========================================

        void BibleFrameSelectButton_Click(object sender, RoutedEventArgs e)
        {
            VM_Option.setBibleFrame();
        }

        void BibleFrameRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            VM_Option.refreshBibleFrame();
        }

        void ReadingFrameSelectButton_Click(object sender, RoutedEventArgs e)
        {
            VM_Option.setReadingFrame();
        }

        void ReadingFrameRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            VM_Option.refreshReadingFrame();
        }

        void SongFrameSelectButton_Click(object sender, RoutedEventArgs e)
        {
            VM_Option.setSongFrame();
        }

        void SongFrameRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongFramePaths_ListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in SongFramePaths_ListBox.SelectedItems)
                itemindex.Add(SongFramePaths_ListBox.Items.IndexOf(item));
            itemindex.Sort();

            VM_Option.refreshSongFrame(itemindex.ToArray());
        }

        void SongFrameDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongFramePaths_ListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in SongFramePaths_ListBox.SelectedItems)
                itemindex.Add(SongFramePaths_ListBox.Items.IndexOf(item));
            itemindex.Sort();

            VM_Option.deleteSongFrame(itemindex.ToArray());
        }

        // ========================================= 숫자기입용 텍스트박스 입력 처리 ============================================

        private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }
    }
}
