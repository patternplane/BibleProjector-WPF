using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace BibleProjector_WPF
{
    public static class SongFrameMenuGenerator
    {
        // =================== Property =================== 

        /// <summary>
        /// 컬렉션 변동 또는 주입된 항목 삭제에 필요한 정보
        /// </summary>
        private static readonly DependencyProperty AssignedCountProperty =
            DependencyProperty.RegisterAttached(
                "AssignedCount",
                typeof(int),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(null));

        /// <summary>
        /// 컬렉션 변동 이벤트 감지를 위한 핸들러
        /// </summary>
        private static readonly DependencyProperty DataUpdateHandlerProperty =
            DependencyProperty.RegisterAttached(
                "DataUpdateHandler",
                typeof(NotifyCollectionChangedEventHandler),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(null));

        private static readonly DependencyProperty DataUpdateLegacyHandlerProperty =
            DependencyProperty.RegisterAttached(
                "DataUpdateLegacyHandler",
                typeof(ListChangedEventHandler),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(null));

        /// <summary>
        /// 이 Behavior를 사용/사용하지 않음
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    true,
                    OnIsEnabledPropertyChanged));

        /// <summary>
        /// 이 ContextMenu를 사용/사용하지 않음
        /// </summary>
        public static readonly DependencyProperty IsActivatedProperty =
            DependencyProperty.RegisterAttached(
                "IsActivated",
                typeof(bool),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    true,
                    OnIsActivatedPropertyChanged));

        /// <summary>
        /// Song Frame 목록을 표시할 인덱스
        /// </summary>
        public static readonly DependencyProperty InsertIndexProperty =
            DependencyProperty.RegisterAttached(
                "InsertIndex",
                typeof(int),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    0,
                    OnBehaviorPropertyChanged));

        /// <summary>
        /// Song Frame 목록을 바인딩할 데이터
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    null,
                    OnItemsSourceChanged));

        /// <summary>
        /// 메뉴 클릭시 응답할 커맨드
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    null,
                    OnBehaviorPropertyChanged));

        /// <summary>
        /// 바인딩한 데이터 내에서 연결할 세부 경로
        /// </summary>
        public static readonly DependencyProperty BindingPathProperty =
            DependencyProperty.RegisterAttached(
                "BindingPath",
                typeof(string),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    null,
                    OnBehaviorPropertyChanged));

        /// <summary>
        /// 바인딩 문자열 포맷
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.RegisterAttached(
                "StringFormat",
                typeof(string),
                typeof(SongFrameMenuGenerator),
                new PropertyMetadata(
                    null,
                    OnBehaviorPropertyChanged));

        // =================== Getter Setter =================== 

        private static int GetAssignedCount(
            DependencyObject element)
        {
            return (int)element.GetValue(AssignedCountProperty);
        }

        private static void SetAssignedCount(
            DependencyObject element,
            int value)
        {
            element.SetValue(AssignedCountProperty, value);
        }

        private static NotifyCollectionChangedEventHandler GetDataUpdateHandler(
            DependencyObject element)
        {
            return (NotifyCollectionChangedEventHandler)element.GetValue(DataUpdateHandlerProperty);
        }

        private static void SetDataUpdateHandler(
            DependencyObject element,
            NotifyCollectionChangedEventHandler value)
        {
            element.SetValue(DataUpdateHandlerProperty, value);
        }

        private static ListChangedEventHandler GetDataUpdateLegacyHandler(
            DependencyObject element)
        {
            return (ListChangedEventHandler)element.GetValue(DataUpdateLegacyHandlerProperty);
        }

        private static void SetDataUpdateLegacyHandler(
            DependencyObject element,
            ListChangedEventHandler value)
        {
            element.SetValue(DataUpdateLegacyHandlerProperty, value);
        }

        public static void SetIsEnabled(
            DependencyObject element,
            bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(
            DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        public static void SetIsActivated(
            DependencyObject element,
            bool value)
        {
            element.SetValue(IsActivatedProperty, value);
        }

        public static bool GetIsActivated(
            DependencyObject element)
        {
            return (bool)element.GetValue(IsActivatedProperty);
        }

        public static void SetInsertIndex(
            DependencyObject element,
            int value)
        {
            element.SetValue(InsertIndexProperty, value);
        }

        public static int GetInsertIndex(
            DependencyObject element)
        {
            return (int)element.GetValue(InsertIndexProperty);
        }

        public static void SetItemsSource(
            DependencyObject element,
            IEnumerable value)
        {
            element.SetValue(ItemsSourceProperty, value);
        }

        public static IEnumerable GetItemsSource(
            DependencyObject element)
        {
            return (IEnumerable)element.GetValue(ItemsSourceProperty);
        }

        public static void SetCommand(
            DependencyObject element,
            ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(
            DependencyObject element)
        {
            return (ICommand)element.GetValue(
                CommandProperty);
        }

        public static void SetBindingPath(
            DependencyObject element,
            string value)
        {
            element.SetValue(BindingPathProperty, value);
        }

        public static string GetBindingPath(
            DependencyObject element)
        {
            return (string)element.GetValue(BindingPathProperty);
        }

        public static void SetStringFormat(
            DependencyObject element,
            string value)
        {
            element.SetValue(StringFormatProperty, value);
        }

        public static string GetStringFormat(
            DependencyObject element)
        {
            return (string)element.GetValue(StringFormatProperty);
        }

        // =================== Events =================== 

        private static void OnIsEnabledPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ContextMenu contextMenu))
                return;
            if (!(e.NewValue is bool isEnabled))
                return;

            // 활성화
            if (isEnabled)
            {
                addCollectionBindingHandler(GetItemsSource(contextMenu), contextMenu);
                RefreshGeneratedItems(contextMenu);
            }

            // 비활성화
            if (!isEnabled)
            {
                removeCollectionBindingHandler(GetItemsSource(contextMenu), contextMenu);
                RemoveGeneratedItems(contextMenu);
            }
        }

        private static void ContextMenuDeactivator(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }

        private static void OnIsActivatedPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement owner))
                return;
            if (!(e.NewValue is bool isActivated))
                return;

            // 활성화
            if (isActivated)
                owner.ContextMenuOpening -= ContextMenuDeactivator;

            // 비활성화
            else
                owner.ContextMenuOpening += ContextMenuDeactivator;
        }

        private static void OnBehaviorPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is ContextMenu contextMenu && GetIsEnabled(contextMenu))
                RefreshGeneratedItems(contextMenu);
        }

        private static void OnItemsSourceChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ContextMenu contextMenu && GetIsEnabled(contextMenu)))
                return;

            // 소스 데이터가 변경되었으므로, 변경 감지 이벤트도 함께 관리해줍니다.
            removeCollectionBindingHandler(e.OldValue, contextMenu);
            addCollectionBindingHandler(e.NewValue, contextMenu);

            RefreshGeneratedItems(contextMenu);
        }

        // =================== Behavior =================== 

        private static void removeCollectionBindingHandler(object mustCollection, ContextMenu contextMenu)
        {
            if (mustCollection is INotifyCollectionChanged notifyCollection)
            {
                NotifyCollectionChangedEventHandler handler = GetDataUpdateHandler(contextMenu);
                if (handler != null)
                    notifyCollection.CollectionChanged -= handler;
            }
            if (mustCollection is IBindingList bindingCollection)
            {
                ListChangedEventHandler handler = GetDataUpdateLegacyHandler(contextMenu);
                if (handler != null)
                    bindingCollection.ListChanged -= handler;
            }
        }

        private static void addCollectionBindingHandler(object mustCollection, ContextMenu contextMenu)
        {
            if (mustCollection is INotifyCollectionChanged notifyCollection)
            {
                NotifyCollectionChangedEventHandler handler = GetDataUpdateHandler(contextMenu);
                if (handler == null)
                {
                    handler = (sender, eventArg) => { RefreshGeneratedItems(contextMenu); };
                    SetDataUpdateHandler(contextMenu, handler);
                }
                notifyCollection.CollectionChanged += handler;
            }
            if (mustCollection is IBindingList bindingCollection)
            {
                ListChangedEventHandler handler = GetDataUpdateLegacyHandler(contextMenu);
                if (handler == null)
                {
                    handler = (sender, eventArg) => { RefreshGeneratedItems(contextMenu); };
                    SetDataUpdateLegacyHandler(contextMenu, handler);
                }
                bindingCollection.ListChanged += handler;
            }
        }

        private static void RefreshGeneratedItems(
            ContextMenu contextMenu)
        {
            RemoveGeneratedItems(contextMenu);

            IEnumerable itemsSource = GetItemsSource(contextMenu);
            if (itemsSource == null)
                return;

            var bindingPath = GetBindingPath(contextMenu);
            var stirngFormat = GetStringFormat(contextMenu);

            var insertIndex = GetInsertIndex(contextMenu);
            insertIndex = Math.Max(0, insertIndex);
            insertIndex = Math.Min(
                insertIndex,
                contextMenu.Items.Count);

            int count = 0;
            foreach (var sourceItem in itemsSource)
            {
                var menuItem = CreateMenuItem(
                    sourceItem,
                    bindingPath,
                    stirngFormat,
                    (sender, e) => {
                        ICommand command = GetCommand(contextMenu);
                        if (command != null)
                            command.Execute(sourceItem);
                    });

                contextMenu.Items.Insert(
                    insertIndex,
                    menuItem);

                insertIndex++;
                count++;
            }

            SetAssignedCount(contextMenu, count);
        }

        private static void RemoveGeneratedItems(
            ContextMenu contextMenu)
        {
            int start = GetInsertIndex(contextMenu);
            int count = GetAssignedCount(contextMenu);

            for (int i = start + count - 1; start <= i; i--)
                contextMenu.Items.RemoveAt(i);
            SetAssignedCount(contextMenu, 0);
        }

        private static MenuItem CreateMenuItem(
            object sourceItem,
            string bindingPath,
            string stringFormat,
            RoutedEventHandler clickEventHandler)
        {
            MenuItem menuItem = new MenuItem{ DataContext = sourceItem };

            if (string.IsNullOrWhiteSpace(bindingPath))
                menuItem.Header = sourceItem;
            else
            {
                menuItem.HeaderStringFormat = stringFormat;
                var headerBinding = new Binding
                {
                    Path = new PropertyPath(bindingPath),
                    Mode = BindingMode.OneWay
                };

                BindingOperations.SetBinding(
                    menuItem,
                    HeaderedItemsControl.HeaderProperty,
                    headerBinding);
            }

            if (clickEventHandler != null)
                menuItem.Click += clickEventHandler;

            return menuItem;
        }
    }
}