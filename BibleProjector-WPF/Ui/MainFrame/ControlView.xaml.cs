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
    /// ControlView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ControlView : UserControl
    {
        public ControlView()
        {
            InitializeComponent();

            Binding b = new Binding("IsLoaded");
            b.Mode = BindingMode.OneWayToSource;
            SetBinding(IsLoadedProperty, b);

            IsLoadedWrapper = ((obj) => IsLoaded);
        }

        public static readonly DependencyProperty IsLoadedProperty =
        DependencyProperty.Register(
            name: "IsLoadedWrapper",
            propertyType: typeof(Predicate<object>),
            ownerType: typeof(ControlView));

        public Predicate<object> IsLoadedWrapper 
        {
            get => (Predicate<object>)GetValue(IsLoadedProperty);
            set => SetValue(IsLoadedProperty, value);
        }
    }
}
