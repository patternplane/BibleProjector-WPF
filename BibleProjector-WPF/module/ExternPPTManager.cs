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

        public ExternPPTManager()
        {
            SetFileDialogs();
        }

        // ================================ 상수 ==============================

        const int MAX_SLIDE_COUNT = 200;

        // ================================== 파일 찾기 ===========================

        static bool isSetUp = false;
        static System.Windows.Forms.OpenFileDialog FD_ExternPPT;

        void SetFileDialogs()
        {
            if (isSetUp)
                return;

            FD_ExternPPT = new System.Windows.Forms.OpenFileDialog();

            FD_ExternPPT.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            FD_ExternPPT.Multiselect = true;
            FD_ExternPPT.Filter = "PowerPoint파일(*.ppt,*.pptx,*.pptm)|*.ppt;*.pptx;*.pptm";

            isSetUp = true;
        }

        // ================================== 메서드 ===========================

        public void ModifyOpenPPT(string[] pptFullPaths)
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

        public void UnlinkPPT(string filePath)
        {
            if (ExternPPTControl.ExternPPTControlAccess != null)
                ExternPPTControl.ExternPPTControlAccess.CheckDeletedPPTAndClose(Path.GetFileName(filePath));
            Powerpoint.ExternPPTs.closeSingle(Powerpoint.EXTERN_TEMP_DIRECTORY + Path.GetFileName(filePath));
        }
        public void UnlinkPPT(string[] pptFullPaths)
        {
            for (int i = 0; i < pptFullPaths.Length; i++)
                UnlinkPPT(pptFullPaths[i]);
        }

        public void RefreshPPT(string[] pptFullPaths)
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

        /// <summary>
        /// 해당 외부ppt파일을 중복/중복이 아니라고 판단하는 함수입니다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public delegate bool isNotOverlaped(string fileName);
        /// <summary>
        /// 사용자 선택 PPT들을 받아, 그 중 사용가능한 ppt 목록을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public string[] getNewValidationPPT(isNotOverlaped checkFunc)
        {
            List<string> successFiles = new List<string>();

            if (FD_ExternPPT.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return successFiles.ToArray();

            bool hasDuplicated = false;
            bool hasTooBigFile = false;
            int errorCode;
            foreach (string fileName in FD_ExternPPT.FileNames)
            {
                errorCode = checkAvailPPT(fileName,checkFunc);

                if (errorCode == 0)
                    successFiles.Add(fileName);
                else if (errorCode == 1)
                    hasTooBigFile = true;
                else
                    hasDuplicated = true;
            }

            if (hasTooBigFile)
                System.Windows.MessageBox.Show("너무 큰 용량의 ppt를 등록하려 했습니다.\r\n파일당 허용하는 최대 슬라이드 수는 " + MAX_SLIDE_COUNT.ToString() + "개 입니다.", "너무 큰 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            if (hasDuplicated)
                System.Windows.MessageBox.Show("하나 이상의 ppt가 이미 등록되어 있었습니다.\r\n중복된 등록은 할 수 없습니다.", "중복 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

            return successFiles.ToArray();
        }

        /// <summary>
        /// ppt파일이 등록 가능한지 판별하여 그 결과 에러코드를 반환합니다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>성공 : 0, 허용 슬라이드 수 초과 : 1, 중복 등록 시도 : 2, 파일 없음 : 3</returns>
        public int checkAvailPPT(string fileName, isNotOverlaped checkFunc)
        {
            if (!(new FileInfo(fileName).Exists))
                return 3;
            else if (!checkFunc(fileName))
                return 2;
            else if (Powerpoint.getSlideCountFromFile(fileName) > MAX_SLIDE_COUNT)
                return 1;
            else
                return 0;
        }
    }
}
