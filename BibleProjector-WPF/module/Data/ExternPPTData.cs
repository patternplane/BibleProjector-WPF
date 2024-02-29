using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module.Data
{
    public class ExternPPTData
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
            // MVVM 위반
            if (ExternPPTControl.ExternPPTControlAccess != null)
                ExternPPTControl.ExternPPTControlAccess.CheckDeletedPPTAndClose(System.IO.Path.GetFileName(fileFullPath));
            Powerpoint.ExternPPTs.closeSingle(Powerpoint.EXTERN_TEMP_DIRECTORY + System.IO.Path.GetFileName(fileFullPath));
        }
    }
}
