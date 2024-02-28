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

        VMReserveData dragPreviewItem;
        VMReserveData dropPreviewItem;
        object[] dragSelection;

        public VMReserveList()
        {
            this.CSetDragDrop = new RelayCommand(SetDragDrop);
            this.CSetDropPreviewPos = new RelayCommand(SetDropPreviewPos);
            this.CApplyDrag = new RelayCommand(ApplyDrag);
            this.CDeleteItems = new RelayCommand(DeleteItems); 

            dragPreviewItem = new VMReserveData("", ReserveViewType.DragPreview);
            dropPreviewItem = new VMReserveData("", ReserveViewType.DropPreview);
            ReserveContents.Add(dropPreviewItem);
            ReserveContents.Add(dragPreviewItem);

            for (int i = 1; i <= 100; i++)
                ReserveContents.Add(new VMReserveData("항목"+i, ReserveViewType.NormalItem));
        }

        // ========== Methods ==========

        void deleteItems(object[] Selection)
        {
            foreach (VMReserveData item in Selection)
                ReserveContents.Remove(item);
        }

        // ========== Command ==========

        void DeleteItems(object Selection)
        {
            deleteItems((object[])Selection);
        }

        // ========== Command - Drag & Drop ==========

        void SetDragDrop(object Selection)
        {
            this.dragSelection = (object[])Selection;
            deleteItems(dragSelection);

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
            for(int i = dragSelection.Length - 1; i >= 0; i--)
                ReserveContents.Insert((int)pos, (VMReserveData)dragSelection[i]);
        }
    }
}
