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

namespace BibleProjector_WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        // =================================================== 프로그램 시작 처리 ======================================================

        public MainWindow(object VM_MainWindow)
        {
            InitializeComponent();
            this.DataContext = VM_MainWindow;

            this.SetBinding(shiftEventManagerProperty, new Binding("shiftEventManager"));
            this.SetBinding(capsLockEventManagerProperty, new Binding("capsLockEventManager"));
            this.SetBinding(keyDownEventManagerProperty, new Binding("keyDownEventManager"));
        }

        // =================================================== 키 상태 광역 전달 ======================================================

        public static readonly DependencyProperty shiftEventManagerProperty =
        DependencyProperty.Register(
            name: "shiftEventManager",
            propertyType: typeof(ViewModel.ShiftEventManager),
            ownerType: typeof(MainWindow));

        public ViewModel.ShiftEventManager shiftEventManager
        {
            get => (ViewModel.ShiftEventManager)GetValue(shiftEventManagerProperty);
            set => SetValue(shiftEventManagerProperty, value);
        }

        public static readonly DependencyProperty capsLockEventManagerProperty =
        DependencyProperty.Register(
            name: "capsLockEventManager",
            propertyType: typeof(ViewModel.CapsLockEventManager),
            ownerType: typeof(MainWindow));

        public ViewModel.CapsLockEventManager capsLockEventManager
        {
            get => (ViewModel.CapsLockEventManager)GetValue(capsLockEventManagerProperty);
            set => SetValue(capsLockEventManagerProperty, value);
        }

        public static readonly DependencyProperty keyDownEventManagerProperty =
        DependencyProperty.Register(
            name: "keyDownEventManager",
            propertyType: typeof(ViewModel.KeyDownEventManager),
            ownerType: typeof(MainWindow));

        public ViewModel.KeyDownEventManager keyDownEventManager
        {
            get => (ViewModel.KeyDownEventManager)GetValue(keyDownEventManagerProperty);
            set => SetValue(keyDownEventManagerProperty, value);
        }

        private void EH_PreKeyDownCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift
                || e.Key == Key.RightShift)
                shiftEventManager.invokeShiftChange(true);
            else if (e.Key == Key.CapsLock)
                capsLockEventManager.invokeCapsLockChange(true);

            keyDownEventManager.invokeKeyDown(e);
        }

        private void EH_KeyUpCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift
                || e.Key == Key.RightShift)
                shiftEventManager.invokeShiftChange(false);
            else if (e.Key == Key.CapsLock)
                capsLockEventManager.invokeCapsLockChange(false);
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
    }
}
