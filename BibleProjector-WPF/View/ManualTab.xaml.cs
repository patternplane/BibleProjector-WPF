using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// ManualTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManualTab : UserControl
    {
        public ManualTab()
        {
            InitializeComponent();

            this.DataContext = new ViewModel.ManualTabViewModel();
            ((ViewModel.ManualTabViewModel)this.DataContext).PropertyChanged += ItemSourceBinder;
            tabUpdate();
        }

        void ItemSourceBinder(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.CompareTo("ManualList") == 0)
                tabUpdate();
        }

        void tabUpdate()
        {
            ManualTabControl.Items.Clear();

            foreach (module.Manual man in ((ViewModel.ManualTabViewModel)this.DataContext).ManualList)
                ManualTabControl.Items.Add(
                    new TabItem()
                    {
                        Header = man.Title
                        ,
                        Content = new ManualContent(man.Content)
                    });
        }
    }
}
