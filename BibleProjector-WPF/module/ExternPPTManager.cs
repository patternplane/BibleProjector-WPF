using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    public class ExternPPTManager
    {

        List<Data.ExternPPTData> ExternPPTList = new List<Data.ExternPPTData>();

        // ================================ 상수 ==============================

        public const int MAX_SLIDE_COUNT = 200;

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
                foreach (Data.ExternPPTData item in ExternPPTList)
                    if (item.fileFullPath.CompareTo(pptFullPath) == 0)
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
            if (!(new FileInfo(pptFullPath).Exists))
                return 2;
            else if (Powerpoint.getSlideCountFromFile(pptFullPath) > MAX_SLIDE_COUNT)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 입력받은 
        /// </summary>
        /// <param name="pptFullPath"></param>
        /// <returns></returns>
        public Data.ExternPPTData AddPPT(string pptFullPath)
        {
            Data.ExternPPTData addedData = new Data.ExternPPTData(pptFullPath);
            ExternPPTList.Add(addedData);
            return addedData;
        }

        public void UnlinkPPT(Data.ExternPPTData item)
        {
            ExternPPTList.Remove(item);
            item.UnlinkPPT();
        }

        public Data.ExternPPTData[] getDataList()
        {
            return ExternPPTList.ToArray();
        }
    }
}
