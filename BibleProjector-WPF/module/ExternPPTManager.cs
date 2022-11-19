using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    internal class ExternPPTManager
    {
        static public void ModifyOpenPPT(string[] pptFullPaths)
        {
            StringBuilder nonFile = new StringBuilder(50);

            for (int i = 0; i < pptFullPaths.Length; i++)
            {
                if (!new FileInfo(pptFullPaths[i]).Exists)
                {
                    nonFile.Append("\r\n");
                    nonFile.Append(pptFullPaths[i]);
                }
                else
                    Powerpoint.justOpen(pptFullPaths[i]);
            }
            if (nonFile.Length != 0)
                System.Windows.MessageBox.Show("해당 PPT파일들은 원본이 없어 열지 못했습니다!" + nonFile.ToString(), "파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }

        static public void UnlinkPPT(string filePath)
        {
            if (ExternPPTControl.ExternPPTControlAccess != null)
                ExternPPTControl.ExternPPTControlAccess.CheckDeletedPPTAndClose(Path.GetFileName(filePath));
            Powerpoint.ExternPPTs.closeSingle(Powerpoint.EXTERN_TEMP_DIRECTORY + Path.GetFileName(filePath));
        }
        static public void UnlinkPPT(string[] pptFullPaths)
        {
            for (int i = 0; i < pptFullPaths.Length; i++)
                UnlinkPPT(pptFullPaths[i]);
        }

        static public void RefreshPPT(string[] pptFullPaths)
        {
            StringBuilder nonFrame = new StringBuilder(50);

            for (int i = 0; i < pptFullPaths.Length; i++)
            {
                if (!new FileInfo(pptFullPaths[i]).Exists)
                {
                    nonFrame.Append("\r\n");
                    nonFrame.Append(pptFullPaths[i]);
                }
                else
                    Powerpoint.ExternPPTs.refreshPresentation(pptFullPaths[i]);
            }
            if (nonFrame.Length != 0)
                System.Windows.MessageBox.Show("해당 PPT파일들은 원본이 없어 새로고침하지 못했습니다!" + nonFrame.ToString(), "파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
