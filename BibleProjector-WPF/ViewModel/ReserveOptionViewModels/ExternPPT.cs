using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.ViewModel.ReserveOptionViewModels
{
    class ExternPPT : NotifyPropertyChanged, IReserveOptionViewModel
    {
        public ExternPPT()
        {
            ReserveManagerViewModel.instance.PropertyChanged += ApplyPropertyChanged;
        }

        // ================================== 속성 =================================

        ReserveCollectionUnit[] selection;

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
                OnPropertyChanged("SlideStartNum_Text");
            }
        }

        // 선택항목의 갯수에 따른 실행버튼 활성화 여부
        bool _ExecuteButtonEnable = false;
        public bool ExecuteButtonEnable { get { return _ExecuteButtonEnable; } set { _ExecuteButtonEnable = value; OnPropertyChanged("ExecuteButtonEnable"); } }

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
            string[] paths = new string[selection.Length];

            int i = 0;
            foreach (ReserveCollectionUnit unit in selection)
                paths[i++] = ((module.ExternPPTReserveDataUnit)unit.reserveData).PPTfilePath;

            return paths;
        }

        public void RunModifyOpenPPT()
        {
            string[] filePaths = getSelectedPaths();
            if (filePaths.Count() == 0)
                return;

            new module.ExternPPTManager().ModifyOpenPPT(filePaths);
        }

        public void RunRefreshPPT()
        {
            string[] filePaths = getSelectedPaths();
            if (filePaths.Count() == 0)
                return;

            new module.ExternPPTManager().RefreshPPT(filePaths);

            ExternPPTControlViewModel vm_ExternPPTControl = new module.ControlWindowManager().getExternPPTControlViewModel();
            if (vm_ExternPPTControl != null)
                foreach (string path in filePaths)
                    vm_ExternPPTControl.RefreshExternPPT(System.IO.Path.GetFileName(path));
        }

        public void ShowContent()
        {
            string[] filePaths = getSelectedPaths();
            if (filePaths.Count() == 0)
                System.Windows.MessageBox.Show("출력할 PPT를 선택해주세요!", "출력할 외부PPT 선택되지 않음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
                new module.ShowStarter().ExternPPTShowStart(
                    System.IO.Path.GetFileName(filePaths[0])
                    , int.Parse(SlideStartNum_Text));
        }

        public void GiveSelection(ReserveCollectionUnit[] data)
        {
            selection = data;
        }
    }
}
