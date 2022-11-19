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

namespace BibleProjector_WPF.ReserveOptionViews
{
    /// <summary>
    /// ExternPPT.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExternPPT : UserControl
    {

        ViewModel.ReserveOptionViewModels.ExternPPT VM_ExternPPT = null;

        public ExternPPT()
        {
            InitializeComponent();

            this.DataContext = VM_ExternPPT = new ViewModel.ReserveOptionViewModels.ExternPPT();
        }

        // ==================================== 항목 파일 열기/새로고침 =================================

        void PPTOpen_Click(object sender, RoutedEventArgs e)
        {
            VM_ExternPPT.RunModifyOpenPPT();
        }

        void Refresh_Click(object sender, RoutedEventArgs e)
        {
            VM_ExternPPT.RunRefreshPPT();
        }

        // ==================================== 실행 =================================

        void PPTRun_Click(object sender, RoutedEventArgs e)
        {
            VM_ExternPPT.PPTRun();
        }
    }
}
