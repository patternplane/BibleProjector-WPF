using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class ExternPPTData : ShowData
    {
        public string fileName { get; private set; }
        public string fileFullPath { get; private set; }

        public ExternPPTData(string pptFullPath)
        {
            this.fileName = System.IO.Path.GetFileNameWithoutExtension(pptFullPath);
            this.fileFullPath = pptFullPath;

            Powerpoint.ExternPPTs.setPresentation(pptFullPath);
        }

        public void ModifyOpenPPT()
        {
            if (!isOriginFileExist())
                throw new Exception("외부PPT 접근 중 원본파일 소실");

            Powerpoint.justOpen(fileFullPath);
        }

        public void RefreshPPT()
        {
            if (!isOriginFileExist())
                throw new Exception("외부PPT 새로고침 중 원본파일 소실");

            Powerpoint.ExternPPTs.refreshPresentation(fileFullPath);

            OnItemRefreshed();
        }

        public bool isOriginFileExist()
        {
            if (new System.IO.FileInfo(fileFullPath).Exists)
                return true;
            else
                return false;
        }

        public void UnlinkPPT()
        {
            Powerpoint.ExternPPTs.closeSingle(Powerpoint.EXTERN_TEMP_DIRECTORY + System.IO.Path.GetFileName(fileFullPath));
        }

        // ================ ShowData 메소드 ================

        public override string getTitle1()
        {
            return "PPT";
        }

        public override string getTitle2()
        {
            return fileName;
        }

        public override ShowContentData[] getContents()
        {
            System.Windows.Media.Imaging.BitmapImage[] contents = Powerpoint.ExternPPTs.getThumbnailImages(fileFullPath);

            ShowContentData[] result = new ShowContentData[contents.Length];
            int i = 0;
            foreach (System.Windows.Media.Imaging.BitmapImage item in contents)
                result[i++] = new ShowContentData(null, item, false);

            return result;
        }

        public override ShowData getNextShowData()
        {
            return null;
        }

        public override ShowData getPrevShowData()
        {
            return null;
        }

        public override ShowContentType getDataType()
        {
            return ShowContentType.PPT;
        }

        public override bool isSameData(ShowData data)
        {
            if (this.GetType() == data.GetType())
                if (this.fileFullPath.CompareTo(((ExternPPTData)data).fileFullPath) == 0)
                    return true;
            return false;
        }

        public override ShowExcuteErrorEnum canExcuteShow()
        {
            return ShowExcuteErrorEnum.NoErrors;
        }

        public override bool isAvailData()
        {
            return true;
        }
    }
}
