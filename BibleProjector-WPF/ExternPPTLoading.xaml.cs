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
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExternPPTLoading : Window
    {
        // 오래 걸리는 작업에 사용할 "대기해주세요" 창인데
        // 개발 덜함

        public ExternPPTLoading()
        {
            InitializeComponent();
        }
    }
}
