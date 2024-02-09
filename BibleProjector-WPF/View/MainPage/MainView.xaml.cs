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

namespace BibleProjector_WPF.View.MainPage
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        System.Reflection.PropertyInfo CPullOptionBarProperty = null;
        void EH_OptionButtonClick(object sender, RoutedEventArgs e)
        {
            if (CPullOptionBarProperty == null)
                CPullOptionBarProperty = (this.DataContext).GetType().GetProperty("CPullOptionBar");

            ((ICommand)(CPullOptionBarProperty.GetValue(this.DataContext))).Execute(null);
        }

        System.Reflection.PropertyInfo CPushOptionBarProperty = null;
        void EH_OptionBarMouseLeave(object sender, RoutedEventArgs e)
        {
            if (CPushOptionBarProperty == null)
                CPushOptionBarProperty = (this.DataContext).GetType().GetProperty("CPushOptionBar");

            ((ICommand)(CPushOptionBarProperty.GetValue(this.DataContext))).Execute(null);
        }
    }
}
