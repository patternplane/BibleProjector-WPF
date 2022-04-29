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
