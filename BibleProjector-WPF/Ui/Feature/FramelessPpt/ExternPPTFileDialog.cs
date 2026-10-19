using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.View.MainPage
{
    public class ExternPPTFileDialog
    {
        static System.Windows.Forms.OpenFileDialog _FD_ExternPPT = null;
        public static System.Windows.Forms.OpenFileDialog FD_ExternPPT 
        { 
            get 
            {
                if (_FD_ExternPPT == null)
                {
                    _FD_ExternPPT = new System.Windows.Forms.OpenFileDialog();
                    _FD_ExternPPT.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
                    _FD_ExternPPT.Filter = "PowerPoint파일(*.ppt,*.pptx,*.pptm)|*.ppt;*.pptx;*.pptm";
                }

                return _FD_ExternPPT;
            }
        }
    }
}
