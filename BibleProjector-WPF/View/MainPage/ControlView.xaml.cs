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

        List<ListBoxItem> selections = new List<ListBoxItem>();
        private void ConfirmSelection(object sender, MouseButtonEventArgs e)
        {
            selections.Clear();
            foreach (ListBoxItem item in ((ListBox)sender).SelectedItems)
                selections.Add(item);
        }

        private void GetMouseMove(object sender, MouseEventArgs e)
        {
            ListBoxItem obj = sender as ListBoxItem;
            if (!selections.Contains(obj))
                return;

            if (obj != null && e.LeftButton == MouseButtonState.Pressed)
            {
                foreach (ListBoxItem item in selections)
                    item.Visibility = Visibility.Collapsed;

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
        const double SCROLL_RANGE = 60.0d;
        private void OnDragOver(object sender, DragEventArgs e)
        {
            // 미리보기 위치 이동

            Point pos_s = e.GetPosition((IInputElement)sender);
            DragInfo dragInfoData = (DragInfo)e.Data.GetData(typeof(DragInfo));

            transformData.X = pos_s.X - dragInfoData.initialPosOffset.X;
            transformData.Y = pos_s.Y + 23 - dragInfoData.initialPosOffset.Y;
            DragPreviewItem.RenderTransform = transformData;
            DragPreviewItem.Visibility = Visibility.Visible;
            DragPreviewItem.IsSelected = true;

            // 드래그 할 위치 찾기

            ListBoxItem NextItem = null; // item : item 바로 앞이 적정위치. null이면 가장 마지막이 적정위치
            foreach(ListBoxItem item in ((ListBox)sender).Items)
            {
                if (item != DragPreviewItem
                    && item != DropPreviewItem
                    && !selections.Contains(item)
                    && e.GetPosition(item).Y - 10 < 0)
                {
                    NextItem = item;
                    break;
                }
            }
            
            if ((NextItem == null && ((ListBox)sender).Items.Count - 1 != ((ListBox)sender).Items.IndexOf(DropPreviewItem))
                || (NextItem != null && ((ListBox)sender).Items.IndexOf(NextItem) - 1 != ((ListBox)sender).Items.IndexOf(DropPreviewItem)))
            {
                ((ListBox)sender).Items.Remove(DropPreviewItem);
                if (NextItem == null)
                    ((ListBox)sender).Items.Add(DropPreviewItem);
                else
                    ((ListBox)sender).Items.Insert(((ListBox)sender).Items.IndexOf(NextItem), DropPreviewItem);
                DropPreviewItem.Visibility = Visibility.Visible;
            }

            // 드래그 중 스크롤 이동

            double verticalPos = e.GetPosition(ReserveScrollViewer).Y;
            double offset;

            if (verticalPos < SCROLL_RANGE) // Top of visible list? 
            {
                offset = 20.0 - verticalPos * 0.3;
                //Scroll up
                ReserveScrollViewer.ScrollToVerticalOffset(ReserveScrollViewer.VerticalOffset - offset);
            }
            else if (verticalPos > ReserveScrollViewer.ActualHeight - SCROLL_RANGE) //Bottom of visible list? 
            {
                offset = 20.0 - (ReserveScrollViewer.ActualHeight - verticalPos) * 0.3;
                //Scroll down
                ReserveScrollViewer.ScrollToVerticalOffset(ReserveScrollViewer.VerticalOffset + offset);
            }
        }
    }
}
