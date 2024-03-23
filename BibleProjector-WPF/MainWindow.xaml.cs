using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ComponentModel;

namespace BibleProjector_WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 개편중 임시로 제작한 구조
        public ViewModel.ViewModel VM_Main { get; set; }

        public static MainWindow ProgramMainWindow = null;

        // =================================================== Shift키 상태 광역 전달 ======================================================

        // 임시로 여기다 해둠, MainViewModel을 도입해야 할 것임
        ViewModel.ShiftEventManager shiftEventManager;
        void CShiftStateChange(object shiftState)
        {
            shiftEventManager.invokeShiftChange((bool)shiftState);
        }

        private void EH_KeyDownCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift
                || e.Key == Key.RightShift)
                CShiftStateChange(true);
        }

        private void EH_KeyUpCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift
                || e.Key == Key.RightShift)
                CShiftStateChange(false);
        }

        // =================================================== 프로그램 시작 처리 ======================================================

        public MainWindow()
        {
            ProgramMainWindow = this;

            InitializeComponent();

            this.DataContext = this;

            //ReserveInitialize();
            // 개편중 임시로 꺼둔 부분임에 유의!

            //===============================================================================================================
            // 개편중 임시로 추가한 부분 -> MainWindow도 역시 ViewModel을 따로 두어 추가처리를 해야하지만 일단 여기서 처치중

            try
            {
                module.ProgramData.Initialize();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "필수 파일 없음", MessageBoxButton.OK, MessageBoxImage.Warning);
                throw new Exception("필수 파일 없음");
            }

            Database.DatabaseInitailize();
            Powerpoint.Initialize();
            module.ProgramOption.Initialize();

            module.ExternPPTManager pptMan = new module.ExternPPTManager();
            module.Data.SongManager songMan = new module.Data.SongManager();
            module.BibleDataManager bibleMan = new module.BibleDataManager();

            module.ISearcher searcher = new module.Data.MultiSearcher(
                new module.BibleSearcher(),
                new module.Data.SongSearcher(
                    songMan),
                new module.ExternPPTSearcher(
                    pptMan));

            shiftEventManager = new ViewModel.ShiftEventManager();

            module.ShowStarter showStarter = new module.ShowStarter();

            System.Collections.ObjectModel.Collection<ViewModel.ViewModel> buttonVMs
                = new System.Collections.ObjectModel.Collection<ViewModel.ViewModel>();
            for (int i = 0; i < 6; i++)
                buttonVMs.Add(new ViewModel.MainPage.VMExternPPTEditButton(pptMan, shiftEventManager, i, showStarter));

            ViewModel.MainPage.VMShowControler[] showControlers = new ViewModel.MainPage.VMShowControler[3];
            showControlers[0] = new ViewModel.MainPage.VMShowControler(ShowContentType.Bible, showStarter);
            showControlers[1] = new ViewModel.MainPage.VMShowControler(ShowContentType.Song, showStarter);
            showControlers[2] = new ViewModel.MainPage.VMShowControler(ShowContentType.PPT, showStarter);

            module.ReserveDataManager reserveDataManager = new module.ReserveDataManager(bibleMan, songMan, pptMan);

            this.VM_Main = new ViewModel.MainPage.VMMain(
                new ViewModel.MainPage.VMControlPage(
                    showControlers,
                    new ViewModel.MainPage.VMSearchControl(searcher, reserveDataManager, showStarter),
                    new ViewModel.MainPage.VMReserveList(reserveDataManager, showStarter),
                    buttonVMs),
                new ViewModel.OptionViewModel(),
                new ViewModel.MainPage.VMOptionBar(),
                new ViewModel.LyricViewModel(showStarter, songMan, reserveDataManager));

            module.ProgramData.SaveDataEvent += songMan.saveData_Lyric;
            module.ProgramData.SaveDataEvent += songMan.saveData_Hymn;
            module.ProgramData.SaveDataEvent += pptMan.saveData;
            module.ProgramData.SaveDataEvent += reserveDataManager.saveData;
            module.ProgramData.SaveDataEvent += module.ProgramOption.saveData;

            //===============================================================================================================
        }

        // =================================================== 프로그램 종료 처리 ======================================================

        ~MainWindow()
        {
            programOut();
        }

        public void programOut()
        {
            module.ProgramData.saveProgramData();
            Powerpoint.FinallProcess();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
