using BibleProjector_WPF.module.Infrastructure;
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

        // ======================= Initialize ======================

        ISourceOfReserve bibleDataManager;
        ISourceOfReserve songDataManager;
        ISourceOfReserve pptDataManager;

        public ReserveDataManager(ISourceOfReserve bibleManager, ISourceOfReserve songManager, ISourceOfReserve pptManager)
        {
            this.bibleDataManager = bibleManager;
            this.songDataManager = songManager;
            this.pptDataManager = pptManager;

            getReserveData();
        }

        private bool isLoading = false;

        void getReserveData()
        {
            isLoading = true;

            string[] rawData = ProgramData.getReserveData()
                .Split(new string[] { "§", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            ReserveType type;
            Data.ShowData data = null;
            for (int i = 0; i < rawData.Length; i += 2)
            {
                type = (ReserveType)int.Parse(rawData[i]);
                if (type == ReserveType.Bible)
                    data = bibleDataManager.getItemByReserveInfo(int.Parse(rawData[i + 1]));
                else if (type == ReserveType.Song)
                    data = songDataManager.getItemByReserveInfo(int.Parse(rawData[i + 1]));
                else if (type == ReserveType.ExternPPT)
                    data = pptDataManager.getItemByReserveInfo(int.Parse(rawData[i + 1]));

                if (data != null)
                {
                    AddReserveItem(this, data);
                    data = null;
                }
            }

            isLoading = false;
        }

        public void saveData(object sender, EventArgs e)
        {
            saveData(true);
        }

        private void saveData(bool isImmidiate)
        {
            // 데이터 불러오는 중의 변경사항은 다시 저장할 필요 없으므로
            // 저장과정 생략
            if (isLoading)
                return;

            StringBuilder str = new StringBuilder(50);
            int type;
            int saveData;
            foreach (Data.ShowData item in reserveList)
            {
                type = -1;
                saveData = -1;
                if (item.getDataType() == ShowContentType.Bible)
                {
                    type = (int)ReserveType.Bible;
                    saveData = bibleDataManager.getReserveInfoByItem(item);
                }
                else if (item.getDataType() == ShowContentType.Song)
                {
                    type = (int)ReserveType.Song;
                    saveData = songDataManager.getReserveInfoByItem(item);
                }
                else if (item.getDataType() == ShowContentType.PPT)
                {
                    type = (int)ReserveType.ExternPPT;
                    saveData = pptDataManager.getReserveInfoByItem(item);
                }

                if (type != -1)
                {
                    str.Append(type);
                    str.Append("§");
                    str.Append(saveData);
                    str.Append("\r\n");
                }
            }

            ProgramData.saveData(SaveDataTypeEnum.ReserveData, str.ToString(), isImmidiate);
        }

        // ======================= List Update Event =======================

        public event Event.ReserveListChangedEventHandler ListChangedEvent;

        // ======================= Reserve Behavior =======================

        public void AddReserveItem(object sender, Data.ShowData data)
        {
            reserveList.Add(data);

            ListChangedEvent?.Invoke(sender, new Event.ReserveListChangedEventArgs(new Data.ShowData[] { data }));
            saveData(false);
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

            deleteItems(dataIdxs);

            ListChangedEvent.Invoke(sender, new Event.ReserveListChangedEventArgs(Event.ReserveListUpdateType.Delete, dataIdxs));
            saveData(false);
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
            saveData(false);
        }
    }
}
