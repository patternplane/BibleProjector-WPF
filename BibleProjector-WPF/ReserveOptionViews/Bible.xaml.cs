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
    /// Bible.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Bible : UserControl
    {
        public Bible()
        {
            InitializeComponent();
        }

        void Event_BibleShow(object sender, RoutedEventArgs e)
        {
            ((ViewModel.ReserveOptionViewModels.Bible)this.DataContext).ShowContent();
        }
    }
}
