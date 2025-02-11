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
    /// AddLyricView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricAddView : UserControl
    {
        public LyricAddView()
        {
            InitializeComponent();

            SetBinding(AddNewLyricCommandProperty, new Binding("CAddNewLyric") { Mode = BindingMode.OneWay });
            SetBinding(CloseCommandProperty, new Binding("CClose") { Mode = BindingMode.OneWay });
            SetBinding(RemoveLinefeedsCommandProperty, new Binding("CRemoveLinefeed") { Mode = BindingMode.OneWay });
        }

        // ================== Binding Properties ==================

        public static readonly DependencyProperty AddNewLyricCommandProperty =
        DependencyProperty.Register(
            name: "AddNewLyricCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(LyricAddView));

        public ICommand AddNewLyricCommand
        {
            get => (ICommand)GetValue(AddNewLyricCommandProperty);
            set => SetValue(AddNewLyricCommandProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty =
        DependencyProperty.Register(
            name: "CloseCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(LyricAddView));

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly DependencyProperty RemoveLinefeedsCommandProperty =
        DependencyProperty.Register(
            name: "RemoveLinefeedsCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(LyricAddView));

        public ICommand RemoveLinefeedsCommand
        {
            get => (ICommand)GetValue(RemoveLinefeedsCommandProperty);
            set => SetValue(RemoveLinefeedsCommandProperty, value);
        }

        // ================== Event Handlers ==================

        private void EH_AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddNewLyricCommand.CanExecute(null))
            {
                AddNewLyricCommand.Execute(null);
                CloseCommand.Execute(null);
            }
        }

        private void EH_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseCommand.Execute(null);
        }

        private void EH_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                CloseCommand.Execute(null);
        }

        private void EH_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                ((UIElement)sender).Focus();
        }

        private void EH_RemoveEnterButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveLinefeedsCommand.Execute(null);
        }
    }
}
