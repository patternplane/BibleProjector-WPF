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
            this.DataContext = VM_Option = new ViewModel.OptionViewModel();
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
            SongFrameDelete();
        }

        void SongFrameListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                SongFrameDelete();
        }

        void SongFrameDelete()
        {
            if (SongFramePaths_ListBox.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("선택된 찬양곡 ppt틀을 삭제하시겠습니까?", "ppt틀 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in SongFramePaths_ListBox.SelectedItems)
                itemindex.Add(SongFramePaths_ListBox.Items.IndexOf(item));
            itemindex.Sort();

            VM_Option.deleteSongFrame(itemindex.ToArray());
        }

        void LayoutResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("모든 창의 크기를 초기화하시겠습니까?", "창 크기 초기화", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            module.LayoutInfo.removeAllLayoutData();

            MainWindow.ProgramMainWindow.ResetLayout();
            ReserveManagerWindow.ReserveWindow.ResetLayout();
            // 구버전 교독문 컨트롤 윈도우 처리부분
            // if (ReadingControl.ReadingControlAccess != null)
            //    ReadingControl.ReadingControlAccess.ResetLayout();
        }
    }
}
