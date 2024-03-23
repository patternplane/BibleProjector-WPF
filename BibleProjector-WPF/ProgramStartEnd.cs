using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF
{
    class ProgramStartEnd
    {
        static ProgramStartEnd s_this = null;

        static public ProgramStartEnd getProgramStartEnd()
        {
            if (s_this == null)
                s_this = new ProgramStartEnd();
            return s_this;
        }

        private ProgramStartEnd()
        {
            if (!isProgramDuplicated())
                Environment.Exit(0);
        }

        // ================== 프로그램 유일성 점검 =================

        const string PROGRAM_FULLNAME = "WorshipProjector-WPF-BSS";
        System.Threading.Mutex programMutex;

        bool isProgramDuplicated()
        {
            bool createdNew = false;
            programMutex = new System.Threading.Mutex(true, PROGRAM_FULLNAME, out createdNew);

            if (!createdNew)
            {
                System.Windows.MessageBox.Show(
                    "프로그램은 한번만 실행하세요!",
                    "프로그램이 이미 작동중",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                Environment.Exit(0);
            }

            return createdNew;
        }

        // ================== 프로그램 시작 작업 =================

        public void doProgramInit()
        {
            //===============================================================================================================

            // 프로그램 로딩 창
            ProgramStartLoading startLodingWindow = new ProgramStartLoading();
            startLodingWindow.Show();

            try
            {
                module.ProgramData.Initialize();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "필수 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                throw new Exception("필수 파일 없음");
            }

            Database.DatabaseInitailize();
            string error = Powerpoint.Initialize();
            if (error.CompareTo("") != 0)
                System.Windows.MessageBox.Show
                    (
                    "다음을 확인해주세요 : \r\n" + error,
                    "ppt틀 등록되지 않음",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information
                    );
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

            ViewModel.ShiftEventManager shiftEventManager = new ViewModel.ShiftEventManager();
            ViewModel.CapsLockEventManager capsLockEventManager = new ViewModel.CapsLockEventManager();

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

            module.ProgramData.SaveDataEvent += songMan.saveData_Lyric;
            module.ProgramData.SaveDataEvent += songMan.saveData_Hymn;
            module.ProgramData.SaveDataEvent += pptMan.saveData;
            module.ProgramData.SaveDataEvent += reserveDataManager.saveData;
            module.ProgramData.SaveDataEvent += module.ProgramOption.saveData;

            new MainWindow(
                new ViewModel.VMMainWindow(
                    new ViewModel.MainPage.VMMain(
                        new ViewModel.MainPage.VMControlPage(
                            showControlers,
                            new ViewModel.MainPage.VMSearchControl(searcher, reserveDataManager, showStarter),
                            new ViewModel.MainPage.VMReserveList(reserveDataManager, showStarter),
                            buttonVMs,
                            capsLockEventManager,
                            showStarter),
                        new ViewModel.OptionViewModel(),
                        new ViewModel.MainPage.VMOptionBar(),
                        new ViewModel.LyricViewModel(showStarter, songMan, reserveDataManager)),
                    shiftEventManager,
                    capsLockEventManager)
                ).Show();
            
            // 프로그램 로딩 창 종료
            startLodingWindow.Close();

            //===============================================================================================================
        }

        // ================== 프로그램 종료 작업 =================

        public void doPostProcess_byNonError()
        {
            Console.WriteLine("프로그램 정상 종료");
        }

        public void doPostProcess_byError()
        {
            Console.WriteLine("프로그램 에러 종료");

            module.ProgramData.saveProgramData();
            Powerpoint.FinallProcess();

            System.Windows.MessageBox.Show("프로그램 결함으로 강제 종료합니다.");
        }
    }
}
