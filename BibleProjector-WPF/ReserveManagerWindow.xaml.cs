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
            this.DataContext = new ReserveManagerViewModel();

            CloseButtonDisabler.DisableCloseButton(this);
        }

        // =========================== Window Setting ============================

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
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        // =========================== Event DataBinding ============================

        void Event_UpButtonClick(object sender, RoutedEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).UpButtonClick();
        }

        void Event_DownButtonClick(object sender, RoutedEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).DownButtonClick();
        }

        void Event_DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).DeleteButtonClick();
        }

        void Event_ReserveListKeyDown(object sender, KeyEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).ListKeyInputed(e);
        }

        void Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).ListSelectionChanged();
        }
    }
}
