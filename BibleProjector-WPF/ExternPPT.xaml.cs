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
    /// ExternPPT.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExternPPT : UserControl
    {
        ViewModel.ExternPPTViewModel VM_ExternPPT = null;

        // 컨트롤
        static public ExternPPTControl Ctrl_ExternPPT = null;

        public ExternPPT()
        {
            InitializeComponent();

            externPptMainGrid.DataContext = VM_ExternPPT = new ViewModel.ExternPPTViewModel();
        }

        // ==================================== 항목 추가/삭제/새로고침 =================================

        void AddExternPPT_Click(object sender, RoutedEventArgs e)
        {
            VM_ExternPPT.RunAddPPT();
        }

        int[] getSelectedIndexes()
        {
            List<int> itemindex = new List<int>(10);
            foreach (object item in ExternPPTListBox.SelectedItems)
                itemindex.Add(ExternPPTListBox.Items.IndexOf(item));
            itemindex.Sort();

            return itemindex.ToArray();
        }

        void PPTOpen_Click(object sender, RoutedEventArgs e)
        {
            if (ExternPPTListBox.SelectedItems.Count == 0)
                return;

            VM_ExternPPT.RunModifyOpenPPT(getSelectedIndexes());
        }

        void PPTDelete_Click(object sender, RoutedEventArgs e)
        {
            ExternPPTDelete();
        }

        void ExternPPTListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                ExternPPTDelete();
        }

        void ExternPPTDelete()
        {
            if (ExternPPTListBox.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show("선택된 PPT를 리스트에서 삭제하시겠습니까?", "외부PPT 등록 해제", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;

            VM_ExternPPT.RunDeletePPT(getSelectedIndexes());
        }

        void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (ExternPPTListBox.SelectedItems.Count == 0)
                return;

            VM_ExternPPT.RunRefreshPPT(getSelectedIndexes());

            if (Ctrl_ExternPPT != null)
                foreach (object obj in ExternPPTListBox.SelectedItems)
                    Ctrl_ExternPPT.RefreshExternPPT(System.IO.Path.GetFileName((string)obj));
        }

        // ==================================== 실행 =================================

        void ExternPPTListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            PPTRun_Click(null, null);
        }

        void PPTRun_Click(object sender, RoutedEventArgs e)
        {
            if (ExternPPTListBox.SelectedIndex == -1)
                MessageBox.Show("출력할 PPT를 선택해주세요!", "출력할 외부PPT 선택되지 않음", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Ctrl_ExternPPT == null)
                {
                    Ctrl_ExternPPT = new ExternPPTControl(
                        VM_ExternPPT.ExternPPTList[ExternPPTListBox.SelectedIndex]
                        , int.Parse(VM_ExternPPT.SlideStartNum_Text)
                        );
                    //Ctrl_ExternPPT.Owner = MainWindow.ProgramMainWindow;
                }
                else
                    Ctrl_ExternPPT.ShowExternPPT(
                        VM_ExternPPT.ExternPPTList[ExternPPTListBox.SelectedIndex]
                        , int.Parse(VM_ExternPPT.SlideStartNum_Text)
                        );
                Ctrl_ExternPPT.Show();
            }
        }
    }
}
