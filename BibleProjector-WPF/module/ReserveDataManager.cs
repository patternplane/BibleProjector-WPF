using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class ReserveDataManager
    {
        List<Data.ShowData> reserveList = new List<Data.ShowData>();

        public Data.ShowData[] getReserveList()
        {
            return reserveList.ToArray();
        }

        // ======================= List Update Event =======================

        public event Event.ReserveListChangedEventHandler ListChangedEvent;

        // ======================= Reserve Behavior =======================

        public void AddReserveItem(object sender, Data.ShowData data)
        {
            reserveList.Add(data);

            ListChangedEvent.Invoke(sender, new Event.ReserveListChangedEventArgs(new Data.ShowData[] { data }));
        }

        void deleteItems(int[] dataIdxs)
        {
            foreach (int idx in dataIdxs)
                reserveList.RemoveAt(idx);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataIdxs">삭제할 인덱스는 반드시 내림차순 정렬이 되어있어야 합니다.</param>
        public void DeleteReserveItem(object sender, int[] dataIdxs)
        {
            for (int i = 0, lastest = dataIdxs[0] + 1; i < dataIdxs.Length; lastest = dataIdxs[i++])
                if (dataIdxs[i] < 0
                    || dataIdxs[i] >= reserveList.Count
                    || lastest <= dataIdxs[i])
                    throw new Exception("예약항목 삭제 정보가 올바르지 않음");

            foreach (int idx in dataIdxs)
                reserveList[idx].deleteProcess();
            deleteItems(dataIdxs);

            ListChangedEvent.Invoke(sender, new Event.ReserveListChangedEventArgs(Event.ReserveListUpdateType.Delete, dataIdxs));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dataIdxs">이동할 인덱스는 반드시 내림차순 정렬이 되어있어야 합니다.</param>
        /// <param name="pos"></param>
        public void MoveReserveItem(object sender, int[] dataIdxs, int pos)
        {
            if (pos < 0 || pos - dataIdxs.Length > reserveList.Count)
                throw new Exception("예약항목 이동 위치가 올바르지 않음");
            for (int i = 0, lastest = dataIdxs[0] + 1; i < dataIdxs.Length; lastest = dataIdxs[i++])
                if (dataIdxs[i] < 0
                    || dataIdxs[i] >= reserveList.Count
                    || lastest <= dataIdxs[i])
                    throw new Exception("예약항목 이동 정보가 올바르지 않음");

            Data.ShowData[] moveitems = new Data.ShowData[dataIdxs.Length];
            for (int i = 0; i < dataIdxs.Length; i++)
                moveitems[i] = reserveList[dataIdxs[i]];
            deleteItems(dataIdxs);
            for (int i = 0; i < moveitems.Length; i++)
                reserveList.Insert(pos, moveitems[i]);

            ListChangedEvent.Invoke(sender, new Event.ReserveListChangedEventArgs(Event.ReserveListUpdateType.Move, dataIdxs, pos));
        }
    }
}
