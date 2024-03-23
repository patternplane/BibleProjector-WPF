using BibleProjector_WPF.module.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class ExternPPTManager : ISourceOfReserve
    {

        ExternPPTData[] ExternPPTList = new ExternPPTData[PPT_LIST_LENGTH];

        // ================================ 상수 ==============================

        public const int MAX_SLIDE_COUNT = 200;
        const int PPT_LIST_LENGTH = 6;

        // ================================== 세팅 / 저장 ===========================

        public ExternPPTManager()
        {
            getSaveData();
        }

        const string SEPARATOR = "∂";

        void getSaveData()
        {
            string[] rawData = ProgramData.getExternPPTData().Split(new string[] { SEPARATOR }, StringSplitOptions.None);

            for (int i = 0; i < PPT_LIST_LENGTH && i < rawData.Length; i++)
                if (CanAddPPT(rawData[i]) == 0)
                    AddPPT(rawData[i],i);
        }

        public void saveData(object sender, Event.SaveDataEventArgs e)
        {
            StringBuilder str = new StringBuilder(10);

            for (int i = 0; i < ExternPPTList.Length; i++)
            {
                if (ExternPPTList[i] != null)
                    str.Append(ExternPPTList[i].fileFullPath);
                if (i < ExternPPTList.Length - 1)
                    str.Append(SEPARATOR);
            }

            e.saveDataFunc(SaveDataTypeEnum.ExternPPTData, str.ToString());
        }

        // ================================== 메서드 ===========================

        /// <summary>
        /// ppt파일이 등록 가능한지 평가하여 상태를 반환합니다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>사용가능 : 0, 허용 슬라이드 수 초과 : 1, 파일 없음 : 2, 중복 등록 시도 : 3</returns>
        public int CanAddPPT(string pptFullPath)
        {
            int validity = validateFile(pptFullPath);
            if (validity == 0)
            {
                foreach (ExternPPTData item in ExternPPTList)
                    if (item != null
                        && item.fileFullPath.CompareTo(pptFullPath) == 0)
                        return 3;
            }

            return validity;
        }

        /// <summary>
        /// ppt파일이 갱신 가능한지 평가하여 상태를 반환합니다.
        /// </summary>
        /// <param name="pptFullPath"></param>
        /// <returns>사용가능 : 0, 허용 슬라이드 수 초과 : 1, 파일 없음 : 2, 중복 등록 시도 : 3</returns>
        public int CanRefreshPPT(string pptFullPath)
        {
            return validateFile(pptFullPath);
        }

        /// <summary>
        /// ppt파일의 사용 가능성을 평가합니다.
        /// </summary>
        /// <param name="pptFullPath"></param>
        /// <returns>사용가능 : 0, 허용 슬라이드 수 초과 : 1, 파일 없음 : 2</returns>
        int validateFile(string pptFullPath)
        {
            FileInfo fi;
            try
            {
                fi = new FileInfo(pptFullPath);
            }
            catch (Exception e)
            {
                return 2;
            }

            if (!fi.Exists)
                return 2;
            else if (Powerpoint.getSlideCountFromFile(pptFullPath) > MAX_SLIDE_COUNT)
                return 1;
            else
                return 0;
        }

        public ExternPPTData AddPPT(string pptFullPath, int posId)
        {
            ExternPPTData addedData = new ExternPPTData(pptFullPath);
            ExternPPTList[posId] = addedData;
            return addedData;
        }

        public void UnlinkPPT(int posId)
        {
            ExternPPTList[posId].UnlinkPPT();
            ExternPPTList[posId] = null;
        }

        public ExternPPTData getMyData(int idx)
        {
            if (idx < 0 || idx >= ExternPPTList.Length)
                return null;
            return ExternPPTList[idx];
        }

        public ExternPPTData[] getDataList()
        {
            List<ExternPPTData> temp = new List<ExternPPTData>(ExternPPTList.Length);
            foreach (ExternPPTData item in ExternPPTList)
                if (item != null)
                    temp.Add(item);
            return temp.ToArray();
        }

        // ================================== 예약값 복원 지원 ===========================

        public ShowData getItemByReserveInfo(int ReserveInfo)
        {
            if (ReserveInfo < 0 || ReserveInfo >= ExternPPTList.Length)
                return null;

            return ExternPPTList[ReserveInfo];
        }

        public int getReserveInfoByItem(ShowData item)
        {
            for (int i = 0; i < ExternPPTList.Length; i++)
                if (ExternPPTList[i] == item)
                    return i;

            return -1;
        }
    }
}
