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
    /// LyricControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricControl : UserControl
    {
        // ViewModel
        ViewModel.LyricViewModel VM_LyricViewModel;

        // 컨트롤
        static public SongControl Ctrl_Song = null;

        // ============================================= 세팅 및 종료 ============================================= 

        public LyricControl()
        {
            InitializeComponent();
            LyricControlMain.DataContext = VM_LyricViewModel = new ViewModel.LyricViewModel();
        }

        // ============================================= 이벤트 ============================================= 

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunSearch();
        }

        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunAdd();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("현재 선택된 곡을 삭제합니다.", "찬양곡 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Hand) != MessageBoxResult.OK)
                return;

            VM_LyricViewModel.RunDelete();
        }

        void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunCompleteModify();
        }

        void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunCompleteModify();
        }

        // ======================================================= 출력 처리 ======================================================

        string[][][] makeSongData(ViewModel.LyricViewModel.SingleLyric lyric, int linePerPage)
        {
            string[] pages = module.StringModifier.makePageWithLines(VM_LyricViewModel.SelectedLyric.content, linePerPage);
            string[][][] songData = new string[pages.Length][][];

            for(int i = 0; i < pages.Length; i++)
            {
                songData[i] = new string[2][];

                songData[i][0] = new string[2];
                songData[i][0][0] = "{t}";
                songData[i][0][1] = lyric.title;

                songData[i][1] = new string[2];
                songData[i][1][0] = "{c}";
                songData[i][1][1] = pages[i];
            }

            return songData;
        }

        void LyricReserveListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            LyricShowButton_Click(null, null);
        }

        void LyricShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM_LyricViewModel.SongFrameSelection == null)
                MessageBox.Show("ppt 틀을 선택해주세요!", "ppt 틀 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (VM_LyricViewModel.SelectedLyric == null)
                MessageBox.Show("출력할 찬양곡을 선택해주세요!", "찬양곡 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                // 곡별 사용할 틀에 대한 설계가 없어 수정되지 않음
                if (Ctrl_Song == null)
                {
                    Ctrl_Song = new SongControl(makeSongData(VM_LyricViewModel.SelectedLyric, VM_LyricViewModel.LinePerSlide), VM_LyricViewModel.SongFrameSelection.Path);
                    Ctrl_Song.Owner = MainWindow.ProgramMainWindow;
                }
                else
                    Ctrl_Song.ShowSong(makeSongData(VM_LyricViewModel.SelectedLyric, VM_LyricViewModel.LinePerSlide), VM_LyricViewModel.SongFrameSelection.Path);
                Ctrl_Song.Show();
            }
        }

        // ======================================================= 숫자만 기입받는 텍스트박스 처리 ======================================================

        private void LinePerSlideTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Delete)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        // ============================================= 예약 특수처리 ============================================= 

        void ReserveUp_Click(object sender, RoutedEventArgs e)
        {
            if (LyricReserveListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in LyricReserveListBox.SelectedItems)
                itemindex.Add(LyricReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            if (itemindex[0] == 0)
                return;

            int i = 0;
            int moveItem;
            int desIndex;
            while (i < itemindex.Count)
            {
                moveItem = itemindex[i] - 1;
                desIndex = itemindex[i];
                for (; i < itemindex.Count && desIndex == itemindex[i]; i++, desIndex++) ;

                VM_LyricViewModel.LyricReserveList.Insert(desIndex, VM_LyricViewModel.LyricReserveList[moveItem]);
                VM_LyricViewModel.LyricReserveList.RemoveAt(moveItem);
            }
        }
        void ReserveDown_Click(object sender, RoutedEventArgs e)
        {
            if (LyricReserveListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in LyricReserveListBox.SelectedItems)
                itemindex.Add(LyricReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            if (itemindex.Last() == VM_LyricViewModel.LyricReserveList.Count - 1)
                return;

            int i = itemindex.Count - 1;
            int moveItem;
            int desIndex;
            while (i >= 0)
            {
                moveItem = itemindex[i] + 1;
                desIndex = itemindex[i];
                for (i--; i >= 0 && desIndex == itemindex[i]; i--, desIndex--) ;

                VM_LyricViewModel.LyricReserveList.Insert(desIndex, VM_LyricViewModel.LyricReserveList[moveItem]);
                VM_LyricViewModel.LyricReserveList.RemoveAt(moveItem + 1);
            }
        }

        void ReserveDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LyricReserveListBox.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("선택된 예약 곡을 삭제합니다.", "찬양곡 예약 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Hand) != MessageBoxResult.OK)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in LyricReserveListBox.SelectedItems)
                itemindex.Add(LyricReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            for (int i = itemindex.Count - 1; i >= 0; i--)
                VM_LyricViewModel.LyricReserveList.Remove(VM_LyricViewModel.LyricReserveList[itemindex[i]]);
        }

        void ReserveAdd_Click(object sender, RoutedEventArgs e)
        {
            if (LyricListBox.SelectedItems.Count == 0)
                return;

            List<int> itemindex = new List<int>(10);
            foreach (object item in LyricListBox.SelectedItems)
                itemindex.Add(LyricListBox.Items.IndexOf(item));
            itemindex.Sort();

            foreach (int i in itemindex)
                VM_LyricViewModel.LyricReserveList.Add(new ViewModel.LyricViewModel.LyricReserve(VM_LyricViewModel.LyricList[i]));
        }
    }
}
