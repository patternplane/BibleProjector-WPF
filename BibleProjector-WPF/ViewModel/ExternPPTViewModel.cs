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

        const string SEPARATOR = "∂";

        public ExternPPTViewModel()
        {
            SetFileDialogs();
            getExternPPTData();
        }

        void getExternPPTData()
        {
            string rawData = module.ProgramData.getExternPPTData(this);

            foreach (string data in rawData.Split(new string[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries))
            {
                ExternPPTList_fullpath.Add(data);
                ExternPPTList.Add(Path.GetFileName(data));
            }

            Powerpoint.ExternPPTs.Initialize(ExternPPTList_fullpath.ToArray());
        }

        public string getSaveData()
        {
            StringBuilder str = new StringBuilder(50);
            foreach (string s in ExternPPTList_fullpath)
            {
                str.Append(s);
                str.Append(SEPARATOR);
            }
            return str.ToString();
        }

        // ================================== 파일 찾기 ===========================

        System.Windows.Forms.OpenFileDialog FD_ExternPPT;

        void SetFileDialogs()
        {
            FD_ExternPPT = new System.Windows.Forms.OpenFileDialog();

            FD_ExternPPT.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            FD_ExternPPT.Multiselect = true;
            FD_ExternPPT.Filter = "PowerPoint파일(*.ppt,*.pptx,*.pptm)|*.ppt;*.pptx;*.pptm";
        }

        // ================================== 속성 =================================

        // 파일 등록 리스트
        public BindingList<string> ExternPPTList { get; set; } = new BindingList<string>();
        private BindingList<string> ExternPPTList_fullpath { get; set; } = new BindingList<string>();

        // 시작 슬라이드 번호 기입
        private string SlideStartNum_Text_in = "1";
        public string SlideStartNum_Text { get { return SlideStartNum_Text_in; } set {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    SlideStartNum_Text_in = "0";
                else
                    SlideStartNum_Text_in = res;
                NotifyPropertyChanged();
            }
        }

        // ================================== 메서드 ==================================

        bool isValidPPT(string path)
        {
            foreach (string file in ExternPPTList_fullpath)
                if (Path.GetFileNameWithoutExtension(file).CompareTo(Path.GetFileNameWithoutExtension(path)) == 0)
                    return false;

            return true;
        }

        public void RunAddPPT()
        {
            if (FD_ExternPPT.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;

            bool hasDuplicated = false;
            foreach (string fileName in FD_ExternPPT.FileNames)
            {
                if (isValidPPT(fileName))
                {
                    Powerpoint.ExternPPTs.setPresentation(fileName);
                    ExternPPTList.Add(Path.GetFileName(fileName));
                    ExternPPTList_fullpath.Add(fileName);
                }
                else
                    hasDuplicated = true;
            }

            if (hasDuplicated)
                System.Windows.MessageBox.Show("하나 이상의 ppt가 이미 등록되어 있었습니다.\r\n중복된 등록은 할 수 없습니다.", "중복 등록", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }

        public void RunDeletePPT(int[] indexList)
        {
            for (int j = indexList.Length - 1; j >= 0; j--)
            {
                Powerpoint.ExternPPTs.closeSingle(Powerpoint.EXTERN_TEMP_DIRECTORY + ExternPPTList[indexList[j]]);
                ExternPPTList.RemoveAt(indexList[j]);
                ExternPPTList_fullpath.RemoveAt(indexList[j]);
            }
        }

        public void RunRefreshPPT(int[] indexList)
        {
            foreach (int i in indexList)
                Powerpoint.ExternPPTs.refreshPresentation(ExternPPTList_fullpath[i]);
        }

    





        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
