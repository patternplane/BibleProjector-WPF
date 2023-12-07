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

using System.ComponentModel;
using BibleProjector_WPF.ViewModel;
using System.Collections.ObjectModel;

namespace BibleProjector_WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        // 예약 창
        ReserveManagerWindow Window_Reserve = null;

        public static MainWindow ProgramMainWindow = null;

        // =================================================== 윈도우 레이아웃 변경 ======================================================

        public void ResetLayout()
        {
            this.Width = 1053.488;
            this.Height = 612.79;
        }

        void changeSize(object sender, SizeChangedEventArgs e)
        {
            module.LayoutInfo.Layout_MainWindow.Width = this.ActualWidth;
            module.LayoutInfo.Layout_MainWindow.Height = this.ActualHeight;
            module.LayoutInfo.Layout_MainWindow.x = this.Left;
            module.LayoutInfo.Layout_MainWindow.y = this.Top;
        }

        void changeLocate(object sender, EventArgs e)
        {
            module.LayoutInfo.Layout_MainWindow.Width = this.ActualWidth;
            module.LayoutInfo.Layout_MainWindow.Height = this.ActualHeight;
            module.LayoutInfo.Layout_MainWindow.x = this.Left;
            module.LayoutInfo.Layout_MainWindow.y = this.Top;
        }

        // =================================================== 프로그램 시작 처리 ======================================================

        public MainWindow()
        {
            ProgramMainWindow = this;

            InitializeComponent();

            ReserveInitialize();

            setLayout();
        }

        void ReserveInitialize()
        {
            Window_Reserve = new ReserveManagerWindow();
            Window_Reserve.Show();
        }

        void setLayout()
        {
            if (module.LayoutInfo.Layout_MainWindow.Width == -1)
                return;

            this.Width = module.LayoutInfo.Layout_MainWindow.Width;
            this.Height = module.LayoutInfo.Layout_MainWindow.Height;
            this.Left = module.LayoutInfo.Layout_MainWindow.x;
            this.Top = module.LayoutInfo.Layout_MainWindow.y;
        }

        // =================================================== 프로그램 종료 처리 ======================================================

        ~MainWindow()
        {
            programOut();
        }

        public void programOut()
        {
            module.ProgramData.saveProgramData();
            Powerpoint.FinallProcess();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            new module.ControlWindowManager().ForceClose();

            // Modal로 사용하는 윈도우인지라 현재 이부분은 필요없음
            //if (SubWindow_BibleModify != null)
            //    SubWindow_BibleModify.ForceClose();
            if (Window_Reserve != null)
                Window_Reserve.ForceClose();

            base.OnClosing(e);
        }
    }
}
