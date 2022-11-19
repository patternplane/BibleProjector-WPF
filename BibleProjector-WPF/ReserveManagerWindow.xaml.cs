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
        // 허허 이게 맞나
        static public ReserveManagerWindow ReserveWindow;

        public ReserveManagerWindow()
        {
            InitializeComponent();
            this.DataContext = new ReserveManagerViewModel();
            ReserveWindow = this;

            CloseButtonDisabler.DisableCloseButton(this);
            setLayout();
        }

        // ============================ 윈도우 레이아웃 ============================

        void setLayout()
        {
            if (module.LayoutInfo.Layout_ReserveWindow.Width == -1)
                return;

            this.Width = module.LayoutInfo.Layout_ReserveWindow.Width;
            this.Height = module.LayoutInfo.Layout_ReserveWindow.Height;
            this.Left = module.LayoutInfo.Layout_ReserveWindow.x;
            this.Top = module.LayoutInfo.Layout_ReserveWindow.y;
        }

        public void ResetLayout()
        {
            this.Width = 501;
            this.Height = 532;
        }

        void changeSize(object sender, SizeChangedEventArgs e)
        {
            module.LayoutInfo.Layout_ReserveWindow.Width = this.ActualWidth;
            module.LayoutInfo.Layout_ReserveWindow.Height = this.ActualHeight;
            module.LayoutInfo.Layout_ReserveWindow.x = this.Left;
            module.LayoutInfo.Layout_ReserveWindow.y = this.Top;
        }

        void changeLocate(object sender, EventArgs e)
        {
            module.LayoutInfo.Layout_ReserveWindow.Width = this.ActualWidth;
            module.LayoutInfo.Layout_ReserveWindow.Height = this.ActualHeight;
            module.LayoutInfo.Layout_ReserveWindow.x = this.Left;
            module.LayoutInfo.Layout_ReserveWindow.y = this.Top;
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
            ((ReserveManagerViewModel)DataContext).PutUpReserveData();
        }

        void Event_DownButtonClick(object sender, RoutedEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).PutDownReserveData();
        }

        void Event_DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            ((ReserveManagerViewModel)DataContext).DeleteReserveData();
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
