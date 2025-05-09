﻿using System;
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

            this.SetBinding(ItemShowStartCommandProperty, new Binding("CItemShowStart"));
            this.SetBinding(ItemSelectionCommandProperty, new Binding("CItemSelection"));
        }

        private void EH_UserControl_Loaded(object sender, RoutedEventArgs e)
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

        public static readonly DependencyProperty ItemShowStartCommandProperty =
        DependencyProperty.Register(
            name: "ItemShowStartCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(ReserverListView));

        public ICommand ItemShowStartCommand
        {
            get => (ICommand)GetValue(ItemShowStartCommandProperty);
            set => SetValue(ItemShowStartCommandProperty, value);
        }

        public static readonly DependencyProperty ItemSelectionCommandProperty =
        DependencyProperty.Register(
            name: "ItemSelectionCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(ReserverListView));

        public ICommand ItemSelectionCommand
        {
            get => (ICommand)GetValue(ItemSelectionCommandProperty);
            set => SetValue(ItemSelectionCommandProperty, value);
        }

        // ========== Reserve View Scroll ==========

        private void EH_ScrollByWheel(object sender, MouseWheelEventArgs e)
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

        List<ViewModel.ViewModel> selections = new List<ViewModel.ViewModel>();
        bool allowSelect = true;

        // ========== Reserve Item Selection Manager ==========

        /// <summary>
        /// 이 이벤트가 SelectionChanged보다 선행되어야 올바른 동작이 될 것임
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EH_ReserveListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selections.Count <= 1)
                allowSelect = true;
            else
                allowSelect = false;

            object VMofClickItem = ((FrameworkElement)e.OriginalSource).DataContext;
            if (!selections.Contains(VMofClickItem))
                selections.Clear();
        }

        private void EH_ReserveListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemSelectionCommand.Execute(e.AddedItems);

            if (allowSelect)
                confirmSelection();
        }

        private void EH_ConfirmSelection(object sender, MouseButtonEventArgs e)
        {
            confirmSelection();
            allowSelect = true;
        }

        void confirmSelection()
        {
            selections.Clear();
            foreach (ViewModel.ViewModel item in ReserveListBox.SelectedItems)
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

        private void EH_ListBoxItem_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
                DeleteSelections();
        }

        private void EH_ListBoxItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ItemShowStartCommand.Execute(((ListBoxItem)sender).DataContext);
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
         *                 Item DoubleClick
         =======================================================*/

        // 변수 도입 사유 :
        //   더블클릭시 새 자막을 표시하면서 ShowControler에 포커싱되어야 하므로
        //   더블클릭의 MouseUp 이벤트에 의한 예약 UI의 포커싱 처리는 무시되어야 함.
        //
        //   그러나 DoubleClick 이벤트의 발생 시점 상,
        //   예약 UI의 포커싱 요청이 모두 끝나기 전에 처리가 되므로
        //   다른 UI에서 거는 포커싱 요청이 무시됨.
        //
        //   따라서 MouseUp 시점에서 최종 더블클릭 처리를 진행하여,
        //   외부와의 간섭을 최소화 하도록 구성함.
        //   이 과정에서 내부 포커싱 로직을 무시하고 더블클릭 상태를 위임할 수 있도록
        //   두 변수를 도입하게 됨.
        private bool ignoreFocusing = false;
        private bool doDoubleClick = false;

        private void EH_ItemDoubleClick_Trigger(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                ignoreFocusing = true;
                doDoubleClick = true;
            }
        }

        private void EH_ItemDoubleClick_Performer(object sender, MouseButtonEventArgs e)
        {
            if (doDoubleClick)
            {
                doDoubleClick = false;
                ItemShowStartCommand.Execute(((ListBoxItem)sender).DataContext);
            }
        }

        /*=======================================================
         *                 Drag And Drop Process
         =======================================================*/

        // ========== Reserve Item Drag & Drop ==========

        private void EH_GetMouseMove(object sender, MouseEventArgs e)
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

                // 미리보기 위치 조정

                dragPreview(e.GetPosition, initialOffset);

                DropPreviewItem.Visibility = Visibility.Visible;
                DragDropEffects result = DragDrop.DoDragDrop(
                    itemUnderMouse,
                    new DataObject(initialOffset),
                    DragDropEffects.Move);
                
                if (result == DragDropEffects.None)
                    DoDragEnd();
            }
        }

        private void EH_DragEnterCheck(object sender, DragEventArgs e)
        {
            prevTime = DateTime.Now;
        }

        private void EH_OnDragOver(object sender, DragEventArgs e)
        {
            // 미리보기 조정

            dragPreview(e.GetPosition, (Point)e.Data.GetData(typeof(Point)));

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

        private delegate Point positionGetter(IInputElement d);
        private TranslateTransform transformData = new TranslateTransform();
        private const double SCROLL_RANGE = 60.0d;
        private DateTime prevTime;
        private void dragPreview(positionGetter pos, Point initPos)
        {
            // 미리보기 위치 이동

            Point pos_s = pos(DragPreviewItem);

            transformData.X = transformData.X + pos_s.X - initPos.X / 2.0;
            transformData.Y = transformData.Y + pos_s.Y - initPos.Y;
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
                    && pos(item).Y - 10 < 0)
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
        }

        private void EH_DragEnd(object sender, DragEventArgs e)
        {
            DoDragEnd();
        }

        private void DoDragEnd()
        {
            int dropIdx = ReserveListBox.ItemContainerGenerator.IndexFromContainer(DropPreviewItem);
            getCApplyDragProperty(ReserveListBox.DataContext).Execute(dropIdx);

            UpdateReserveItemIdxs();

            ReserveListBox.UpdateLayout();
            foreach (object item in selections)
                ((ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(item)).IsSelected = true;
            if (selections.Count > 0)
                ((ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(selections[0])).Focus();

            DragPreviewItem.Visibility = Visibility.Hidden;
            DropPreviewItem.Visibility = Visibility.Collapsed;

            allowSelect = true;
        }

        /*=======================================================
         *                 UI Focusing Process
         =======================================================*/

        private void EH_ReserveListBox_Focus_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ignoreFocusing)
            {
                ignoreFocusing = false;
                return;
            }

            ListBoxItem mustFocusedItem = null;

            foreach (object item in ReserveListBox.SelectedItems)
            {
                if (getViewTypeProperty(item) == ReserveViewType.NormalItem)
                {
                    mustFocusedItem = (ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(item);
                    break;
                }
            }

            if (mustFocusedItem == null)
            {
                foreach (object item in ReserveListBox.Items)
                {
                    if (getViewTypeProperty(item) == ReserveViewType.NormalItem)
                    {
                        mustFocusedItem = (ListBoxItem)ReserveListBox.ItemContainerGenerator.ContainerFromItem(item);
                        break;
                    }
                }
            }

            if (mustFocusedItem != null)
            {
                // 기능 설명 :
                //   기본적으로 포커싱이 적용되면 스크롤은 자동으로 이동함.
                //   그러나 종종 다른 작업 중 마우스를 사용하여
                //   예약창의 스크롤 바를 이동하고자 할 때,
                //   스크롤이 자동으로 이동되어 의도치 않은 스크롤 튐 현상이 발생함.
                //  
                //   이 현상을 막기 위해 도입.
                doNotBringIntoView = true;
                mustFocusedItem.Focus();
            }
        }

        private bool doNotBringIntoView = false;
        private void EH_ListBoxItem_Focus_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = doNotBringIntoView;
            doNotBringIntoView = false;
        }
    }
}
