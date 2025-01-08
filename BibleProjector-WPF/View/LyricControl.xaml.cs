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
        LyricViewModel VM_LyricViewModel { get { return (LyricViewModel)this.DataContext; } }

        // ============================================= 세팅 및 종료 ============================================= 

        public LyricControl()
        {
            InitializeComponent();
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

        private void LyricContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.submitModifyingCurrentContent();
        }

        private void RemoveEnterButton_Click(object sender, RoutedEventArgs e)
        {
            VM_LyricViewModel.RunRemoveDoubleEnter();
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
    }
}
