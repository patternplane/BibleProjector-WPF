using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class ReserveDataManager
    {
        private const string SAVE_FILE_PREFIX = "data:reserve:1.0";

        List<Data.ReserveData> reserveList = new List<Data.ReserveData>();

        public Data.ReserveData[] getReserveList()
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

            loadData();
        }

        private bool isLoading = false;

        void loadData()
        {
            string rawData = ProgramData.getReserveData();
            if (rawData.StartsWith(SAVE_FILE_PREFIX))
                loadReserveData(rawData.Substring(SAVE_FILE_PREFIX.Length));
            else
                loadReserveData_Legacy(rawData);
        }

        void loadReserveData(string dataFromFile)
        {
            isLoading = true;

            Reserve.SaveData[] saveDataList = System.Text.Json.JsonSerializer.Deserialize<Reserve.SaveData[]>(dataFromFile);

            foreach (Reserve.SaveData saveData in saveDataList)
            {
                Data.ShowData data = null;
                switch(saveData.type)
                {
                    case ReserveType.Bible:
                        data = bibleDataManager.getItemByReserveInfo(saveData.dataCode);
                        break;
                    case ReserveType.Song:
                        data = songDataManager.getItemByReserveInfo(saveData.dataCode);
                        break;
                    case ReserveType.ExternPPT:
                        data = pptDataManager.getItemByReserveInfo(saveData.dataCode);
                        break;
                }

                if (data != null)
                {
                    var frame = ProgramOption.getFrameByPath(saveData.framePath);
                    AddReserveItem(this, new Data.ReserveData(data, songFrame: frame));
                }
            }

            isLoading = false;
        }

        [Obsolete("이 메서드는 더 이상 사용되지 않습니다.")]
        void loadReserveData_Legacy(string dataFromFile)
        {
            isLoading = true;

            string[] rawData = dataFromFile
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

        [Obsolete("이 메서드는 더 이상 사용되지 않습니다.")]
        private string makeSaveData_Legacy()
        {
            StringBuilder str = new StringBuilder(50);
            int type;
            int saveData;
            foreach (Data.ReserveData reserveData in reserveList)
            {
                Data.ShowData item = reserveData.data;
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

            return str.ToString();
        }

        private string makeSaveData()
        {
            Reserve.SaveData[] dataList = new Reserve.SaveData[reserveList.Count];
            for (int i = 0; i < reserveList.Count; i++)
            {
                int dataCode = -1;
                ReserveType type = ReserveType.NULL;
                switch(reserveList[i].data.getDataType())
                {
                    case ShowContentType.Bible:
                        dataCode = bibleDataManager.getReserveInfoByItem(reserveList[i].data);
                        type = ReserveType.Bible;
                        break;
                    case ShowContentType.Song:
                        dataCode = songDataManager.getReserveInfoByItem(reserveList[i].data);
                        type = ReserveType.Song;
                        break;
                    case ShowContentType.PPT:
                        dataCode = pptDataManager.getReserveInfoByItem(reserveList[i].data);
                        type = ReserveType.ExternPPT;
                        break;
                }

                dataList[i] = new Reserve.SaveData(type, dataCode, reserveList[i].songFrame?.Path);
            }

            return SAVE_FILE_PREFIX + System.Text.Json.JsonSerializer.Serialize(dataList);
        }

        private void saveData(bool isImmidiate)
        {
            // 데이터 불러오는 중의 변경사항은 다시 저장할 필요 없으므로
            // 저장과정 생략
            if (isLoading)
                return;

            ProgramData.saveData(SaveDataTypeEnum.ReserveData, makeSaveData(), isImmidiate);
        }

        // ======================= List Update Event =======================

        public event Event.ReserveListChangedEventHandler ListChangedEvent;

        // ======================= Reserve Behavior =======================

        public void AddReserveItem(object sender, Data.ShowData data)
        {
            AddReserveItem(sender, new Data.ReserveData(data, null));
        }

        public void AddReserveItem(object sender, Data.ReserveData data)
        {
            reserveList.Add(data);

            ListChangedEvent?.Invoke(sender, new Event.ReserveListChangedEventArgs(new Data.ReserveData[] { data }));
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

            Data.ReserveData[] moveitems = new Data.ReserveData[dataIdxs.Length];
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
