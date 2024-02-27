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
        public ICommand CStartDragDrop { get; set; }
        public ICommand CSetDropPreviewPos { get; set; }
        public ICommand CApplyDrag { get; set; }

        VMReserveData dragPreviewItem;
        VMReserveData dropPreviewItem;
        object[] dragSelection;

        public VMReserveList()
        {
            this.CStartDragDrop = new RelayCommand(StartDragDrop);
            this.CSetDropPreviewPos = new RelayCommand(SetDropPreviewPos);
            this.CApplyDrag = new RelayCommand(ApplyDrag);

            dragPreviewItem = new VMReserveData("", ReserveViewType.DragPreview);
            dropPreviewItem = new VMReserveData("", ReserveViewType.DropPreview);

            ReserveContents.Add(new VMReserveData("항목1", ReserveViewType.NormalItem));
            ReserveContents.Add(new VMReserveData("항목2", ReserveViewType.NormalItem));
            ReserveContents.Add(new VMReserveData("항목3", ReserveViewType.NormalItem));
            ReserveContents.Add(dropPreviewItem);
            ReserveContents.Add(dragPreviewItem);
            ReserveContents.Add(new VMReserveData("항목4", ReserveViewType.NormalItem));
            for (int i = 5; i <= 100; i++)
                ReserveContents.Add(new VMReserveData("항목"+i, ReserveViewType.NormalItem));
        }

        void StartDragDrop(object Selection)
        {
            this.dragSelection = (object[])Selection;
            foreach(VMReserveData item in dragSelection)
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
            for(int i = dragSelection.Length - 1; i >= 0; i--)
                ReserveContents.Insert((int)pos, (VMReserveData)dragSelection[i]);
        }
    }
}
