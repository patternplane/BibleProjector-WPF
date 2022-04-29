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

        // ============================================= 세팅 ============================================= 

        public LyricControl()
        {
            InitializeComponent();
            LyricControlMain.DataContext = VM_LyricViewModel = new ViewModel.LyricViewModel();
        }

        // ============================================= 이벤트 ============================================= 

        public void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunSearch();
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunAdd();
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunDelete();
        }

        public void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunCompleteModify();
        }

        public void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunCompleteModify();
        }

        // ============================================= 예약 특수처리 ============================================= 

        void ReserveUp_Click(object sender, RoutedEventArgs e)
        {
            int count = LyricReserveListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = LyricReserveListBox.Items.IndexOf(LyricReserveListBox.SelectedItems[0]);
            foreach (object item in LyricReserveListBox.SelectedItems)
            {
                if (startidx > LyricReserveListBox.Items.IndexOf(item))
                    startidx = LyricReserveListBox.Items.IndexOf(item);
            }

            if (startidx != 0)
            {
                VM_LyricViewModel.LyricReserveList.Insert(startidx + count, VM_LyricViewModel.LyricReserveList[startidx - 1]);
                VM_LyricViewModel.LyricReserveList.Remove(VM_LyricViewModel.LyricReserveList[startidx - 1]);
            }
        }
        void ReserveDown_Click(object sender, RoutedEventArgs e)
        {
            int count = LyricReserveListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = LyricReserveListBox.Items.IndexOf(LyricReserveListBox.SelectedItems[0]);
            foreach (object item in LyricReserveListBox.SelectedItems)
            {
                if (startidx > LyricReserveListBox.Items.IndexOf(item))
                    startidx = LyricReserveListBox.Items.IndexOf(item);
            }

            if (startidx + count != VM_LyricViewModel.LyricReserveList.Count)
            {
                ViewModel.LyricViewModel.LyricReserve item = VM_LyricViewModel.LyricReserveList[startidx + count];
                VM_LyricViewModel.LyricReserveList.Remove(item);
                VM_LyricViewModel.LyricReserveList.Insert(startidx, item);
            }
        }

        void ReserveDelete_Click(object sender, RoutedEventArgs e)
        {
            int count = LyricReserveListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = LyricReserveListBox.Items.IndexOf(LyricReserveListBox.SelectedItems[0]);
            foreach (object item in LyricReserveListBox.SelectedItems)
            {
                if (startidx > LyricReserveListBox.Items.IndexOf(item))
                    startidx = LyricReserveListBox.Items.IndexOf(item);
            }

            for (; count > 0; count--)
                VM_LyricViewModel.LyricReserveList.Remove(VM_LyricViewModel.LyricReserveList[startidx]);
        }

        void ReserveAdd_Click(object sender, RoutedEventArgs e)
        {
            int count = LyricListBox.SelectedItems.Count;
            if (count == 0)
                return;

            int startidx = LyricListBox.Items.IndexOf(LyricListBox.SelectedItems[0]);
            foreach (object item in LyricListBox.SelectedItems)
            {
                if (startidx > LyricListBox.Items.IndexOf(item))
                    startidx = LyricListBox.Items.IndexOf(item);
            }

            for (int i = 0; i < count; i++)
                VM_LyricViewModel.LyricReserveList.Add(new ViewModel.LyricViewModel.LyricReserve( VM_LyricViewModel.LyricList[startidx + i]));
        }
    }
}
