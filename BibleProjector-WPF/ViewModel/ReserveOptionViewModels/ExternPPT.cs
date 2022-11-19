using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    class ExternPPT : INotifyPropertyChanged
    {
        // 컨트롤
        static public ExternPPTControl Ctrl_ExternPPT = null;

        public ExternPPT()
        {
            ReserveManagerViewModel.instance.PropertyChanged += ApplyPropertyChanged;
        }

        // ================================== 속성 =================================

        // 시작 슬라이드 번호 기입
        private string SlideStartNum_Text_in = "1";
        public string SlideStartNum_Text
        {
            get { return SlideStartNum_Text_in; }
            set
            {
                string res = module.StringModifier.makeOnlyNum(value);
                if (res.Length == 0)
                    SlideStartNum_Text_in = "0";
                else
                    SlideStartNum_Text_in = res;
                NotifyPropertyChanged("SlideStartNum_Text");
            }
        }

        // 선택항목의 갯수에 따른 실행버튼 활성화 여부
        bool _ExecuteButtonEnable = false;
        public bool ExecuteButtonEnable { get { return _ExecuteButtonEnable; } set { _ExecuteButtonEnable = value; NotifyPropertyChanged("ExecuteButtonEnable"); } }

        // ================================== 속성 변경 바인딩 =================================

        void ApplyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "selectionType")
            {
                if (((ReserveManagerViewModel)sender).selectionType == ReserveSelectionsType.ExternPPT_Single)
                    ExecuteButtonEnable = true;
                else
                    ExecuteButtonEnable = false;
            }
        }

        // ================================== 메서드 ==================================

        string[] getSelectedPaths()
        {
            List<string> filePaths = new List<string>(10);
            if (ReserveManagerViewModel.instance.getTypeOfSelection() == ReserveSelectionsType.ExternPPT_Single
                || ReserveManagerViewModel.instance.getTypeOfSelection() == ReserveSelectionsType.ExternPPT_Multi)
            {
                foreach (module.ExternPPTReserveDataUnit item in ReserveManagerViewModel.instance.getSelectionItems())
                    filePaths.Add(item.PPTfilePath);
            }
            return filePaths.ToArray();
        }

        public void RunModifyOpenPPT()
        {
            string[] filePaths = getSelectedPaths();
            if (filePaths.Count() == 0)
                return;

            module.ExternPPTManager.ModifyOpenPPT(filePaths);
        }

        public void RunRefreshPPT()
        {
            string[] filePaths = getSelectedPaths();
            if (filePaths.Count() == 0)
                return;

            module.ExternPPTManager.RefreshPPT(filePaths);

            if (Ctrl_ExternPPT != null)
                foreach (string path in filePaths)
                    Ctrl_ExternPPT.RefreshExternPPT(System.IO.Path.GetFileName(path));
        }

        public void PPTRun()
        {
            string[] filePaths = getSelectedPaths();
            if (filePaths.Count() == 0)
                System.Windows.MessageBox.Show("출력할 PPT를 선택해주세요!", "출력할 외부PPT 선택되지 않음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
            {
                if (Ctrl_ExternPPT == null)
                {
                    Ctrl_ExternPPT = new ExternPPTControl(
                        filePaths[0]
                        , int.Parse(SlideStartNum_Text)
                        );
                    //Ctrl_ExternPPT.Owner = MainWindow.ProgramMainWindow;
                }
                else
                    Ctrl_ExternPPT.ShowExternPPT(
                        filePaths[0]
                        , int.Parse(SlideStartNum_Text)
                        );
                Ctrl_ExternPPT.Show();
            }
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
