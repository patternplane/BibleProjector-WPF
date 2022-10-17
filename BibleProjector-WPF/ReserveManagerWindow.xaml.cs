using BibleProjector_WPF.ViewModel;
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

using System.ComponentModel;

namespace BibleProjector_WPF
{
    /// <summary>
    /// ReserveManagerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReserveManagerWindow : Window
    {
        public ReserveManagerWindow()
        {
            InitializeComponent();
        }
        public ReserveManagerWindow(System.Collections.IEnumerable reservdata)
        {
            InitializeComponent();

            BibleReserveListBox2.ItemsSource = reservdata;
            BibleReserveListBox2.DisplayMemberPath = "DisplayData";
        }
    }
}
