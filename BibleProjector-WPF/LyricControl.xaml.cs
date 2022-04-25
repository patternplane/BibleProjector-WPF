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
    }
}
