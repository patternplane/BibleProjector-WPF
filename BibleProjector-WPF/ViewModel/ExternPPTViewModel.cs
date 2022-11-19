using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.IO;

namespace BibleProjector_WPF.ViewModel
{
    class ExternPPTViewModel
    {

        public ExternPPTViewModel()
        {
            SetFileDialogs();
        }

        // ================================ 상수 ==============================

        const int MAX_SLIDE_COUNT = 200;

        // ================================== 파일 찾기 ===========================

        System.Windows.Forms.OpenFileDialog FD_ExternPPT;

        void SetFileDialogs()
        {
            FD_ExternPPT = new System.Windows.Forms.OpenFileDialog();

            FD_ExternPPT.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            FD_ExternPPT.Multiselect = true;
            FD_ExternPPT.Filter = "PowerPoint파일(*.ppt,*.pptx,*.pptm)|*.ppt;*.pptx;*.pptm";
        }

        // ================================== 메서드 ==================================

        public void RunAddPPT()
        {
            if (FD_ExternPPT.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            bool hasDuplicated = false;
            bool hasTooBigFile = false;
            foreach (string fileName in FD_ExternPPT.FileNames)
            {
                if (Powerpoint.getSlideCountFromFile(fileName) > MAX_SLIDE_COUNT)
                    hasTooBigFile = true;
                else if (ReserveManagerViewModel.instance.ExternPPT_isNotOverlaped(fileName))
                {
                    Powerpoint.ExternPPTs.setPresentation(fileName);
                    ReserveManagerViewModel.instance.AddReserveData(
                        new module.ExternPPTReserveDataUnit(fileName));
                }
                else
                    hasDuplicated = true;
            }

            if (hasTooBigFile)
                System.Windows.MessageBox.Show("너무 큰 용량의 ppt를 등록하려 했습니다.\r\n파일당 허용하는 최대 슬라이드 수는 " + MAX_SLIDE_COUNT.ToString() + "개 입니다.", "너무 큰 파일 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            if (hasDuplicated)
                System.Windows.MessageBox.Show("하나 이상의 ppt가 이미 등록되어 있었습니다.\r\n중복된 등록은 할 수 없습니다.", "중복 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
