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
    /// ReserverListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReserverListView : UserControl
    {
        public ReserverListView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReserveListDragDropInit();
        }

        // ========== ReserveList Initialize ==========

        ListBoxItem DragPreviewItem;
        ListBoxItem DropPreviewItem;

        void ReserveListDragDropInit()
        {
            foreach (object item in ReserveListBox.Items)
            {
                if (getViewTypeProperty(item) == ReserveViewType.DragPreview)
                    DragPreviewItem = (ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(item);
                if (getViewTypeProperty(item) == ReserveViewType.DropPreview)
                    DropPreviewItem = (ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(item);
            }
            DragPreviewItem.Visibility = Visibility.Hidden;
            DropPreviewItem.Visibility = Visibility.Collapsed;

            UpdateReserveItemIdxs();
        }

        // ========== ReserveItem BindingProperties ==========

        System.Reflection.PropertyInfo _ViewTypeProperty = null;
        ReserveViewType getViewTypeProperty(object obj)
        {
            if (_ViewTypeProperty == null)
                _ViewTypeProperty = obj.GetType().GetProperty("ViewType")
                    ?? throw new Exception("Binding Error");

            return (ReserveViewType)_ViewTypeProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _MyIdxProperty = null;
        void setMyIdxProperty(object obj, object value)
        {
            if (_MyIdxProperty == null)
                _MyIdxProperty = obj.GetType().GetProperty("MyIdx")
                    ?? throw new Exception("Binding Error");

            _MyIdxProperty.SetValue(obj, value);
        }

        // ========== ReserveList BindingProperties ==========

        System.Reflection.PropertyInfo _CSetDropPreviewPosProperty = null;
        ICommand getCSetDropPreviewPosProperty(object obj)
        {
            if (_CSetDropPreviewPosProperty == null)
                _CSetDropPreviewPosProperty = obj.GetType().GetProperty("CSetDropPreviewPos")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CSetDropPreviewPosProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CSetDragDropProperty = null;
        ICommand getCSetDragDropProperty(object obj)
        {
            if (_CSetDragDropProperty == null)
                _CSetDragDropProperty = obj.GetType().GetProperty("CSetDragDrop")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CSetDragDropProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CApplyDragProperty = null;
        ICommand getCApplyDragProperty(object obj)
        {
            if (_CApplyDragProperty == null)
                _CApplyDragProperty = obj.GetType().GetProperty("CApplyDrag")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CApplyDragProperty.GetValue(obj);
        }

        System.Reflection.PropertyInfo _CDeleteItemsProperty = null;
        ICommand getCDeleteItemsProperty(object obj)
        {
            if (_CDeleteItemsProperty == null)
                _CDeleteItemsProperty = obj.GetType().GetProperty("CDeleteItems")
                    ?? throw new Exception("Binding Error");

            return (ICommand)_CDeleteItemsProperty.GetValue(obj);
        }

        // ========== Reserve View Scroll ==========

        private void ScrollByWheel(object sender, MouseWheelEventArgs e)
        {
            ReserveScrollViewer.ScrollToVerticalOffset(ReserveScrollViewer.VerticalOffset - 0.5 * e.Delta);
        }

        // ========== Reserve Item Index Updater ==========

        void UpdateReserveItemIdxs()
        {
            int viewIdx = 1;
            for (int i = 0; i < ReserveListBox.Items.Count; i++)
                if (getViewTypeProperty(ReserveListBox.Items[i]) != ReserveViewType.DragPreview
                    && getViewTypeProperty(ReserveListBox.Items[i]) != ReserveViewType.DropPreview)
                    setMyIdxProperty(ReserveListBox.Items[i], viewIdx++);
        }

        // ========== Reserve Item Selection Data ==========

        List<object> selections = new List<object>();
        bool allowSelect = true;

        // ========== Reserve Item Selection Manager ==========

        /// <summary>
        /// 이 이벤트가 SelectionChanged보다 선행되어야 올바른 동작이 될 것임
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReserveListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selections.Count <= 1)
                allowSelect = true;
            else
                allowSelect = false;

            object VMofClickItem = ((FrameworkElement)e.OriginalSource).DataContext;
            if (!selections.Contains(VMofClickItem))
                selections.Clear();
        }

        private void ReserveListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allowSelect)
                confirmSelection();
        }

        private void ConfirmSelection(object sender, MouseButtonEventArgs e)
        {
            confirmSelection();
            allowSelect = true;
        }

        void confirmSelection()
        {
            selections.Clear();
            foreach (object item in ReserveListBox.SelectedItems)
                if (getViewTypeProperty(item) != ReserveViewType.DragPreview
                    && getViewTypeProperty(item) != ReserveViewType.DropPreview)
                    selections.Add(item);
            selections.Sort(
                (x, y) =>
                {
                    return ReserveListBox.Items.IndexOf(x)
                    .CompareTo(ReserveListBox.Items.IndexOf(y));
                });
        }

        /*=======================================================
         *                   Delete Process
         =======================================================*/

        private void ListBoxItem_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
                DeleteSelections();
        }

        private void ReserveListBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteSelections();
        }

        void DeleteSelections()
        {
            if (selections.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    selections.Count + "개 항목을 삭제합니다"
                    , "삭제"
                    , MessageBoxButton.OKCancel
                    , MessageBoxImage.Warning);

                if (result == MessageBoxResult.Cancel)
                    return;

                getCDeleteItemsProperty(ReserveListBox.DataContext).Execute(selections.ToArray());
                UpdateReserveItemIdxs();
            }
        }

        /*=======================================================
         *                 Drag And Drop Process
         =======================================================*/

        // ========== Reserve Item Drag & Drop ==========

        private void GetMouseMove(object sender, MouseEventArgs e)
        {
            ListBoxItem itemUnderMouse = (ListBoxItem)sender;

            if (itemUnderMouse == DropPreviewItem
                || itemUnderMouse == DragPreviewItem
                || !selections.Contains(itemUnderMouse.DataContext))
                return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                allowSelect = false;

                Point initialOffset = e.GetPosition(itemUnderMouse);

                getCSetDragDropProperty(ReserveListBox.DataContext).Execute(selections.ToArray());

                DropPreviewItem.Visibility = Visibility.Visible;
                DragDropEffects result = DragDrop.DoDragDrop(
                    itemUnderMouse,
                    new DataObject(initialOffset),
                    DragDropEffects.Move);
                
                if (result == DragDropEffects.None)
                    DoDragEnd();
            }
        }

        private void DragEnterCheck(object sender, DragEventArgs e)
        {
            prevTime = DateTime.Now;
        }

        TranslateTransform transformData = new TranslateTransform();
        const double SCROLL_RANGE = 60.0d;
        DateTime prevTime;
        private void OnDragOver(object sender, DragEventArgs e)
        {
            // 미리보기 위치 이동

            Point initialPosOffset = (Point)e.Data.GetData(typeof(Point));
            Point pos_s = e.GetPosition(DragPreviewItem);

            transformData.X = transformData.X + pos_s.X - initialPosOffset.X / 2.0;
            transformData.Y = transformData.Y + pos_s.Y - initialPosOffset.Y;
            DragPreviewItem.RenderTransform = transformData;
            DragPreviewItem.Visibility = Visibility.Visible;
            DragPreviewItem.IsSelected = true;

            // 드롭 할 위치 찾기

            ItemContainerGenerator reserveContainerGenerator = ReserveListBox.ItemContainerGenerator;
            int NextItemIdx = 0; // NextItemIdx 항목의 앞자리가 적정위치임을 표시
            for (; NextItemIdx < reserveContainerGenerator.Items.Count; NextItemIdx++)
            {
                ListBoxItem item = (ListBoxItem)reserveContainerGenerator.ContainerFromIndex(NextItemIdx);
                if (getViewTypeProperty(ReserveListBox.Items[NextItemIdx]) != ReserveViewType.DragPreview
                    && getViewTypeProperty(ReserveListBox.Items[NextItemIdx]) != ReserveViewType.DropPreview
                    && e.GetPosition(item).Y - 10 < 0)
                {
                    break;
                }
            }

            if (NextItemIdx == 0
                || getViewTypeProperty(ReserveListBox.Items[NextItemIdx - 1]) != ReserveViewType.DropPreview)
            {
                object item = reserveContainerGenerator.ItemFromContainer(DropPreviewItem);

                getCSetDropPreviewPosProperty(ReserveListBox.DataContext).Execute(NextItemIdx);
                ReserveListBox.UpdateLayout();

                DropPreviewItem = (ListBoxItem)reserveContainerGenerator.ContainerFromItem(item);
            }

            // 드래그 중 스크롤 이동

            double verticalPos = e.GetPosition(ReserveScrollViewer).Y;
            double offset;

            DateTime currentTime = DateTime.Now;
            TimeSpan timeIntv = currentTime - prevTime;
            if (verticalPos < SCROLL_RANGE) // Top of visible list? 
            {
                offset = (20.0 - verticalPos * 0.3) * (timeIntv.TotalMilliseconds / 50.0);
                //Scroll up
                ReserveScrollViewer.ScrollToVerticalOffset(ReserveScrollViewer.VerticalOffset - offset);
            }
            else if (verticalPos > ReserveScrollViewer.ActualHeight - SCROLL_RANGE) //Bottom of visible list? 
            {
                offset = (20.0 - (ReserveScrollViewer.ActualHeight - verticalPos) * 0.3) * (timeIntv.TotalMilliseconds / 50.0);
                //Scroll down
                ReserveScrollViewer.ScrollToVerticalOffset(ReserveScrollViewer.VerticalOffset + offset);
            }
            prevTime = currentTime;
        }

        private void DragEnd(object sender, DragEventArgs e)
        {
            DoDragEnd();
        }

        void DoDragEnd()
        {
            int dropIdx = ReserveListBox.ItemContainerGenerator.IndexFromContainer(DropPreviewItem);
            getCApplyDragProperty(ReserveListBox.DataContext).Execute(dropIdx);

            UpdateReserveItemIdxs();

            ReserveListBox.UpdateLayout();
            foreach (object item in selections)
                ((ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(item)).IsSelected = true;

            DragPreviewItem.Visibility = Visibility.Hidden;
            DropPreviewItem.Visibility = Visibility.Collapsed;

            allowSelect = true;
        }
    }
}
