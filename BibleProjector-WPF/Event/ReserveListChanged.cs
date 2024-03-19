using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.Event
{
    public enum ReserveListUpdateType
    {
        Add,
        Delete,
        Move
    }

    public class ReserveListChangedEventArgs : EventArgs
    {
        public ReserveListChangedEventArgs(module.Data.ShowData[] newData) 
        {
            this.updateType = ReserveListUpdateType.Add;
            this.updatedObjects = newData;
        }

        public ReserveListChangedEventArgs(ReserveListUpdateType updateType, int[] updateData, int moveIdx = -1)
        {
            if (updateType == ReserveListUpdateType.Move
                && moveIdx == -1)
                throw new Exception("예약 리스트의 이동 이벤트는 이동 위치를 함께 알려야 합니다.");

            this.updateType = updateType;
            this.updatedObjects = updateData;
            this.moveIdx = moveIdx;
        }

        public ReserveListUpdateType updateType;
        public object updatedObjects;
        public int moveIdx = -1;
    }

    public delegate void ReserveListChangedEventHandler(object sender, ReserveListChangedEventArgs e);
}
