using BibleProjector_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // ============================================= 세팅 및 종료 ============================================= 

        public LyricControl()
        {
            InitializeComponent();
            LyricControlMain.DataContext = VM_LyricViewModel = new ViewModel.LyricViewModel();
        }

        // ============================================= 이벤트 ============================================= 

        // =========================== 곡 선택 탭

        void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunSearch();
        }

        void SearchComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                VM_LyricViewModel.RunSearch();
        }

        void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
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

        // =========================== 곡 추가 탭

        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunAdd();
        }

        // =========================== 찬송가 탭

        void HymnContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunApplyHymnModify();
        }

        // ======================================================= 출력 처리 ======================================================

        void LyricReserveListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            VM_LyricViewModel.RunShowLyric();
        }

        void LyricShowButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunShowLyric();
        }

        // ============================================ 예약처리 ==========================================
        
        void ThisLyricReserve_Click(object sender, RoutedEventArgs e) 
        {
            VM_LyricViewModel.RunAddReserveFromSelection();
        }

        // ============================================= 예약 특수처리 ============================================= 

        Collection<ReserveCollectionUnit> getSortedSelection(ItemCollection items, System.Collections.IList rawSelection)
        {
            List<int> itemindex = new List<int>(rawSelection.Count);

            for (int i = 0; i < rawSelection.Count; i++)
                itemindex.Add(items.IndexOf(rawSelection[i]));
            itemindex.Sort();

            Collection<ReserveCollectionUnit> sortedItems = new Collection<ReserveCollectionUnit>();
            foreach (int i in itemindex)
                sortedItems.Add((ReserveCollectionUnit)items[i]);

            return sortedItems;
        }

        void ReserveUp_Click(object sender, RoutedEventArgs e)
        {
            if (LyricReserveListBox.SelectedItems.Count == 0)
                return;

            ViewModel.ReserveDataManager.instance.moveUpInCategory(
                getSortedSelection(LyricReserveListBox.Items, LyricReserveListBox.SelectedItems));

            /*if (itemindex[0] == 0)
                return;

            int i = 0;
            int moveItem;
            int desIndex;
            while (i < itemindex.Count)
            {
                moveItem = itemindex[i] - 1;
                desIndex = itemindex[i];
                for (; i < itemindex.Count && desIndex == itemindex[i]; i++, desIndex++) ;

                *//*VM_LyricViewModel.LyricReserveList.Insert(desIndex, VM_LyricViewModel.LyricReserveList[moveItem]);
                VM_LyricViewModel.LyricReserveList.RemoveAt(moveItem);*//*
            }*/
        }

        void ReserveDown_Click(object sender, RoutedEventArgs e)
        {
            if (LyricReserveListBox.SelectedItems.Count == 0)
                return;

            ViewModel.ReserveDataManager.instance.moveDownInCategory(
                getSortedSelection(LyricReserveListBox.Items, LyricReserveListBox.SelectedItems));

            /*List<int> itemindex = new List<int>(10);
            foreach (object item in LyricReserveListBox.SelectedItems)
                itemindex.Add(LyricReserveListBox.Items.IndexOf(item));
            itemindex.Sort();

            *//*if (itemindex.Last() == VM_LyricViewModel.LyricReserveList.Count - 1)
                return;*//*

            int i = itemindex.Count - 1;
            int moveItem;
            int desIndex;
            while (i >= 0)
            {
                moveItem = itemindex[i] + 1;
                desIndex = itemindex[i];
                for (i--; i >= 0 && desIndex == itemindex[i]; i--, desIndex--) ;

                *//*VM_LyricViewModel.LyricReserveList.Insert(desIndex, VM_LyricViewModel.LyricReserveList[moveItem]);
                VM_LyricViewModel.LyricReserveList.RemoveAt(moveItem + 1);*//*
            }*/
        }

        void ReserveDelete_Click(object sender, RoutedEventArgs e)
        {
            ReserveDelete();
        }

        void ReserveDelete_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                ReserveDelete();
        }

        void ReserveDelete()
        {
            if (LyricReserveListBox.SelectedItems.Count == 0)
                return;
            if (MessageBox.Show("선택된 예약 곡을 삭제하시겠습니까?", "찬양곡 예약 삭제", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;

            ViewModel.ReserveDataManager.instance.deleteItems(
                getSortedSelection(LyricReserveListBox.Items, LyricReserveListBox.SelectedItems));

            /*List<int> itemindex = new List<int>(10);
            foreach (object item in LyricReserveListBox.SelectedItems)
                itemindex.Add(LyricReserveListBox.Items.IndexOf(item));
            itemindex.Sort();*/

            /*for (int i = itemindex.Count - 1; i >= 0; i--)
                VM_LyricViewModel.LyricReserveList.Remove(VM_LyricViewModel.LyricReserveList[itemindex[i]]);*/
        }

        void ReserveAdd_Click(object sender, RoutedEventArgs e)
        {
            if (LyricReserveSelectTabControl.SelectedIndex == 0)
            {
                if (LyricListBox.SelectedItems.Count == 0)
                    return;

                List<int> itemindex = new List<int>(10);
                foreach (object item in LyricListBox.SelectedItems)
                    itemindex.Add(LyricListBox.Items.IndexOf(item));
                itemindex.Sort();

                foreach (int i in itemindex)
                {
                    /*VM_LyricViewModel.LyricReserveList.Add(new ViewModel.LyricViewModel.LyricReserve(ViewModel.LyricViewModel.LyricList[i]));*/

                    // 예약창에 보내는 데이터
                    ViewModel.ReserveManagerViewModel.instance.ReserveDataManager.addReserve(
                        new module.SongReserveDataUnit(ViewModel.LyricViewModel.LyricList[i]));
                }
            }
            if (LyricReserveSelectTabControl.SelectedIndex == 1)
            {
                if (HymnListBox.SelectedItems.Count == 0)
                    return;

                List<int> itemindex = new List<int>(10);
                foreach (object item in HymnListBox.SelectedItems)
                    itemindex.Add(HymnListBox.Items.IndexOf(item));
                itemindex.Sort();

                foreach (int i in itemindex)
                {
                    /*VM_LyricViewModel.LyricReserveList.Add(new ViewModel.LyricViewModel.LyricReserve(ViewModel.LyricViewModel.HymnList[i]));*/

                    // 예약창에 보내는 데이터
                    ViewModel.ReserveManagerViewModel.instance.ReserveDataManager.addReserve(
                        new module.SongReserveDataUnit(ViewModel.LyricViewModel.HymnList[i]));
                }
            }
        }
    }
}
