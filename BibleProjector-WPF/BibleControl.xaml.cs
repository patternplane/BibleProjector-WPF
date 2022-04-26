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
    /// BibleControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BibleControl : Window
    {
        private ViewModel.BibleControlViewModel VM_BibleControl;

        public BibleControl(string Kjjeul)
        {
            InitializeComponent();
            this.DataContext = VM_BibleControl = new ViewModel.BibleControlViewModel(Kjjeul);
        }

        // ================================================ 이벤트 처리 ================================================ 

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Right:
                    VM_BibleControl.RunNextPage();
                    break;
                case Key.Down:
                case Key.Left:
                    VM_BibleControl.RunPreviousPage();
                    break;
            }
        }

        void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_BibleControl.RunPreviousPage();
        }

        void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            VM_BibleControl.RunNextPage();
        }
    }
}
