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
    /// BibleModifyWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BibleModifyWindow : Window
    {
        ViewModel.BibleModifyViewModel VM_BibleModify = null;

        public BibleModifyWindow(string Kjjeul)
        {
            InitializeComponent();
            this.DataContext = VM_BibleModify = new ViewModel.BibleModifyViewModel(Kjjeul);
        }

        public void setData(string Kjjeul)
        {
            VM_BibleModify.setData(Kjjeul);
        }

        // =============================== 버튼 클릭 ================================

        void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            VM_BibleModify.save();
        }

        void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            VM_BibleModify.reset();
        }
        
        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // ========================================== 윈도우 처리 =====================================
        // Modal로 사용하는 윈도우인지라 이 부분은 현재 필요 없음.

        /*
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
                e.Cancel = true;
            }
            base.OnClosing(e);
        }
        */
    }
}
