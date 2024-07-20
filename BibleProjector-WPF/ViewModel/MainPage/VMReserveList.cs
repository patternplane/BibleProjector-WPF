using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BibleProjector_WPF.ViewModel.MainPage
{
    public class VMReserveList : ViewModel
    {
        public ObservableCollection<VMReserveData> ReserveContents { get; set; } = new ObservableCollection<VMReserveData>();
        public ICommand CSetDragDrop { get; set; }
        public ICommand CSetDropPreviewPos { get; set; }
        public ICommand CApplyDrag { get; set; }
        public ICommand CDeleteItems { get; set; }
        public ICommand CItemShowStart { get; set; }
        public ICommand CItemSelection { get; set; }

        VMReserveData dragPreviewItem;
        VMReserveData dropPreviewItem;

        private BibleSelectionEventManager bibleSelectionEventManager;

        module.ReserveDataManager reserveDataManager;
        module.ShowStarter showStarter;

        public VMReserveList(module.ReserveDataManager reserveDataManager, module.ShowStarter showStarter, BibleSelectionEventManager bibleSelectionEventManager)
        {
            this.CSetDragDrop = new RelayCommand((obj) => SetDragDrop((ViewModel[])obj));
            this.CSetDropPreviewPos = new RelayCommand(SetDropPreviewPos);
            this.CApplyDrag = new RelayCommand(ApplyDrag);
            this.CDeleteItems = new RelayCommand((obj) => DeleteItems((ViewModel[])obj));
            this.CItemShowStart = new RelayCommand((obj) => ItemShowStart((VMReserveData)obj));
            this.CItemSelection = new RelayCommand((obj) => ItemSelection((System.Collections.IList)obj));

            reserveDataManager.ListChangedEvent += OnReserveDataUpdated;
            this.reserveDataManager = reserveDataManager;
            this.showStarter = showStarter;

            this.bibleSelectionEventManager = bibleSelectionEventManager;

            dragPreviewItem = new VMReserveData("", ReserveViewType.DragPreview);
            dropPreviewItem = new VMReserveData("", ReserveViewType.DropPreview);

            ReserveContents.Clear();
            ReserveContents.Add(dropPreviewItem);
            ReserveContents.Add(dragPreviewItem);
            foreach (module.Data.ShowData item in reserveDataManager.getReserveList())
                ReserveContents.Add(new VMReserveData(item));
        }

        void OnReserveDataUpdated(object sender, Event.ReserveListChangedEventArgs e)
        {
            if (sender != this)
            {
                if (e.updateType == Event.ReserveListUpdateType.Add)
                    foreach (module.Data.ShowData data in (module.Data.ShowData[])e.updatedObjects)
                        ReserveContents.Add(new VMReserveData(data) { MyIdx = ReserveContents .Count - 1});
                else if (e.updateType == Event.ReserveListUpdateType.Delete)
                {
                    int dragPreviewIdx = ReserveContents.IndexOf(dragPreviewItem);
                    int dropPreviewIdx = ReserveContents.IndexOf(dropPreviewItem);
                    
                    foreach (int itemIdx in (int[])e.updatedObjects)
                        ReserveContents.RemoveAt(itemIdx + (dragPreviewIdx < itemIdx ? 1 : 0) + (dropPreviewIdx < itemIdx ? 1 : 0));
                }
                else if (e.updateType == Event.ReserveListUpdateType.Move)
                {
                    int[] moveIdxs = (int[])e.updatedObjects;

                    int dragPreviewIdx = ReserveContents.IndexOf(dragPreviewItem);
                    int dropPreviewIdx = ReserveContents.IndexOf(dropPreviewItem);

                    VMReserveData[] moveList = new VMReserveData[moveIdxs.Length];
                    for (int i = 0, moveItemIdx; i < moveIdxs.Length; i++)
                    {
                        moveItemIdx = moveIdxs[i] + (dragPreviewIdx < moveIdxs[i] ? 1 : 0) + (dropPreviewIdx < moveIdxs[i] ? 1 : 0);
                        moveList[i] = ReserveContents[moveItemIdx];
                        ReserveContents.RemoveAt(moveItemIdx);
                    }

                    int movePos = e.moveIdx + (dragPreviewIdx < e.moveIdx ? 1 : 0) + (dropPreviewIdx < e.moveIdx ? 1 : 0);
                    for (int i = 0; i < moveIdxs.Length; i++)
                        ReserveContents.Insert(movePos, moveList[i]);
                }
            }
        }

        int[] getDataIndexes(ViewModel[] itemViewModels)
        {
            int[] deleteItems = new int[itemViewModels.Length];

            int dragPreviewIdx = ReserveContents.IndexOf(dragPreviewItem);
            int dropPreviewIdx = ReserveContents.IndexOf(dropPreviewItem);
            for (int i = 0, itemIdx; i < itemViewModels.Length; i++)
            {
                itemIdx = ReserveContents.IndexOf((VMReserveData)itemViewModels[i]);
                itemIdx -= (dragPreviewIdx < itemIdx ? 1 : 0) + (dropPreviewIdx < itemIdx ? 1 : 0);
                deleteItems[i] = itemIdx;
            }
            Array.Sort(deleteItems, (a, b) => b.CompareTo(a));

            return deleteItems;
        }

        // ========== Command ==========

        void DeleteItems(ViewModel[] Selection)
        {
            int[] deleteItems = getDataIndexes(Selection);

            foreach (VMReserveData item in Selection)
                ReserveContents.Remove(item);
            reserveDataManager.DeleteReserveItem(this, deleteItems);
        }

        void ItemShowStart(VMReserveData data)
        {
            showStarter.Show(data.Data);
        }

        private void ItemSelection(System.Collections.IList items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
                if (((VMReserveData)items[i]).ContentType == ShowContentType.Bible)
                {
                    module.Data.BibleData data = (module.Data.BibleData)((VMReserveData)items[i]).Data;
                    bibleSelectionEventManager.InvokeBibleSelection(data.book, data.chapter, data.verse);
                    return;
                }
        }

        // ========== Command - Drag & Drop ==========

        int[] dragSelectionIndexes;
        ViewModel[] dragSelection;

        void SetDragDrop(ViewModel[] Selection)
        {
            this.dragSelection = Selection;
            dragSelectionIndexes = getDataIndexes(dragSelection);

            foreach (VMReserveData item in dragSelection)
                ReserveContents.Remove(item);

            dragPreviewItem.pasteDisplayTitle(((VMReserveData)dragSelection[0]).DisplayTitle);
        }

        void SetDropPreviewPos(object pos)
        {
            int nextItemIdx = (int)pos;
            int dropItemIdx = ReserveContents.IndexOf(dropPreviewItem);

            if (dropItemIdx != -1 && dropItemIdx < nextItemIdx)
                ReserveContents.Move(dropItemIdx, nextItemIdx - 1);
            else
                ReserveContents.Move(dropItemIdx, nextItemIdx);
        }

        void ApplyDrag(object pos)
        {
            for (int i = dragSelection.Length - 1; i >= 0; i--)
                ReserveContents.Insert((int)pos, (VMReserveData)dragSelection[i]);

            int inListPos = (int)pos;
            if (ReserveContents.IndexOf(dropPreviewItem) < (int)pos)
                inListPos--;
            if (ReserveContents.IndexOf(dragPreviewItem) < (int)pos)
                inListPos--;

            reserveDataManager.MoveReserveItem(this, dragSelectionIndexes, inListPos);
        }
    }
}
