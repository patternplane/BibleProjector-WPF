using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private Thread monitorDetector = null;

        public MainWindow(object VM_MainWindow)
        {
            InitializeComponent();
            this.DataContext = VM_MainWindow;

            this.SetBinding(keyInputEventManagerProperty, new Binding("keyInputEventManager"));
            this.SetBinding(windowActivateChangedEventManagerProperty, new Binding("windowActivateChangedEventManager"));
            this.SetBinding(activeSetterProperty, new Binding("activeSetter") { Mode = BindingMode.OneWay });

            setMinSize();
            setInnerContentSize();

            monitorDetector = new Thread(monitorSizeDetector) { IsBackground = true };
            monitorDetector.Start();
        }

        // =================================================== 윈도우 리사이징 처리 ======================================================

        private const double DEFALT_W = 1475.0;
        private const double DEFALT_H = 830.0;

        private void monitorSizeDetector()
        {
            double monitorHeight = SystemParameters.PrimaryScreenHeight;
            double monitorWidth = SystemParameters.PrimaryScreenWidth;

            Action task = new Action(this.setMinSize);

            while (true)
            {
                if (monitorHeight != SystemParameters.PrimaryScreenHeight
                    || monitorWidth != SystemParameters.PrimaryScreenWidth)
                {
                    this.Dispatcher.BeginInvoke(task);
                    monitorHeight = SystemParameters.PrimaryScreenHeight;
                    monitorWidth = SystemParameters.PrimaryScreenWidth;
                }

                Thread.Sleep(10);
            }
        }

        private void setMinSize()
        {
            double ratio = 0.8;

            if (DEFALT_H / DEFALT_W > SystemParameters.PrimaryScreenHeight / SystemParameters.PrimaryScreenWidth)
            {
                double h = SystemParameters.PrimaryScreenHeight * ratio;
                WindowContent.MinHeight = h;
                WindowContent.MinWidth = h * DEFALT_W / DEFALT_H;
            }
            else
            {
                double w = SystemParameters.PrimaryScreenWidth * ratio;
                WindowContent.MinHeight = w * DEFALT_H / DEFALT_W;
                WindowContent.MinWidth = w;
            }

            this.MinHeight = WindowContent.MinHeight + (this.ActualHeight - WindoeInnerGrid.ActualHeight);
            this.MinWidth = WindowContent.MinWidth + (this.ActualWidth - WindoeInnerGrid.ActualWidth);
        }

        private void setInnerContentSize()
        {
            double h = WindowContent.ActualHeight;
            double w = WindowContent.ActualWidth;

            if (h == 0 || w == 0)
                return;

            foreach (FrameworkElement f in RatioSizeElements)
            {
                if (h / w > DEFALT_H / DEFALT_W)
                {
                    f.Height = h / w * DEFALT_W;
                    f.Width = DEFALT_W;
                }
                else
                {
                    f.Height = DEFALT_H;
                    f.Width = w / h * DEFALT_H;
                }
            }
        }

        private void EH_WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            setMinSize();
            setInnerContentSize();
        }

        public static readonly DependencyProperty RatioSizeProperty =
            DependencyProperty.RegisterAttached(
              "RatioSize",
              typeof(bool),
              typeof(MainWindow),
              new PropertyMetadata(false, RatioSizeCallback)
            );

        public static bool GetRatioSize(UIElement target) => 
            (bool)target.GetValue(RatioSizeProperty);

        public static void SetRatioSize(UIElement target, bool value) =>
            target.SetValue(RatioSizeProperty, value);

        private static void RatioSizeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement && (bool)e.NewValue && !RatioSizeElements.Contains(d))
            {
                RatioSizeElements.Add((FrameworkElement)d);
            }
            else if (d is FrameworkElement && !(bool)e.NewValue && RatioSizeElements.Contains(d))
            {
                RatioSizeElements.Remove((FrameworkElement)d);
            }
        }

        private static List<FrameworkElement> RatioSizeElements = new List<FrameworkElement>();

        // =================================================== 키 상태 광역 전달 ======================================================

        public static readonly DependencyProperty keyInputEventManagerProperty =
        DependencyProperty.Register(
            name: "keyInputEventManager",
            propertyType: typeof(Event.KeyInputEventManager),
            ownerType: typeof(MainWindow));

        public Event.KeyInputEventManager keyInputEventManager
        {
            get => (Event.KeyInputEventManager)GetValue(keyInputEventManagerProperty);
            set => SetValue(keyInputEventManagerProperty, value);
        }

        private void EH_PreKeyDownCheck(object sender, KeyEventArgs e)
        {
            foreach (TrackedKey k in TrackedKeys)
                if (k.key == e.Key)
                    k.state = true;

            keyInputEventManager.invokeKeyInput(e.Key, true, e.IsRepeat);
        }

        private void EH_KeyUpCheck(object sender, KeyEventArgs e)
        {
            foreach (TrackedKey k in TrackedKeys)
                if (k.key == e.Key)
                    k.state = false;

            keyInputEventManager.invokeKeyInput(e.Key, false, e.IsRepeat);
        }

        private class TrackedKey
        {
            public Key key;
            public bool state;
        }

        private TrackedKey[] TrackedKeys =
        {
            new TrackedKey() { key = Key.LeftShift, state = false},
            new TrackedKey() { key = Key.RightShift, state = false},
            new TrackedKey() { key = Key.LeftCtrl, state = false},
            new TrackedKey() { key = Key.RightCtrl, state = false},
            new TrackedKey() { key = Key.CapsLock, state = false}
        };

        // =================================================== 윈도우 활성화 상태 처리 ======================================================

        public static readonly DependencyProperty windowActivateChangedEventManagerProperty =
        DependencyProperty.Register(
            name: "windowActivateChangedEventManager",
            propertyType: typeof(Event.WindowActivateChangedEventManager),
            ownerType: typeof(MainWindow));

        public Event.WindowActivateChangedEventManager windowActivateChangedEventManager
        {
            get => (Event.WindowActivateChangedEventManager)GetValue(windowActivateChangedEventManagerProperty);
            set => SetValue(windowActivateChangedEventManagerProperty, value);
        }

        public static readonly DependencyProperty activeSetterProperty =
        DependencyProperty.Register(
            name: "activeSetter",
            propertyType: typeof(bool),
            ownerType: typeof(MainWindow),
            typeMetadata: new PropertyMetadata(
                (d, e) =>
                {
                    if ((bool)e.NewValue)
                    {
                        ((MainWindow)d).Topmost = true;
                        ((MainWindow)d).Topmost = false;
                        ((MainWindow)d).Activate();
                    }
                }
                )
            );

        public bool activeSetter
        {
            get => (bool)GetValue(activeSetterProperty);
            set => SetValue(activeSetterProperty, value);
        }

        private void EH_Window_Activated(object sender, EventArgs e)
        {
            foreach (TrackedKey k in TrackedKeys) 
                if (Keyboard.IsKeyDown(k.key) != k.state)
                    keyInputEventManager.invokeKeyInput(k.key, Keyboard.IsKeyDown(k.key), false);

            windowActivateChangedEventManager.invoke(true);
        }

        private void EH_Window_Deactivated(object sender, EventArgs e)
        {
            windowActivateChangedEventManager.invoke(false);
        }
    }
}
