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
        }

        class DragInfo
        {
            public object dragSource;
            public object data;
            public Point initialPosOffset;

            public DragInfo(object dragSource, object data, Point initialPosOffset)
            {
                this.dragSource = dragSource;
                this.data = data;
                this.initialPosOffset = initialPosOffset;
            }
        }

        private void GetMouseMove(object sender, MouseEventArgs e)
        {
            ListBoxItem obj = sender as ListBoxItem;

            if (obj != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(
                    obj,
                    new DataObject(
                        new DragInfo(
                            obj,
                            obj.DataContext,
                            e.GetPosition(obj))),
                    DragDropEffects.Move);
            }
        }

        TranslateTransform transformData = new TranslateTransform();
        private void OnDragOver(object sender, DragEventArgs e)
        {
            Point pos_s = e.GetPosition((IInputElement)sender);
            DragInfo dragInfoData = (DragInfo)e.Data.GetData(typeof(DragInfo));

            transformData.X = pos_s.X - dragInfoData.initialPosOffset.X;
            transformData.Y = pos_s.Y + dragInfoData.initialPosOffset.Y;
            temp.RenderTransform = transformData;
            temp.Visibility = Visibility.Visible;
            temp.IsSelected = true;
        }
    }
}
