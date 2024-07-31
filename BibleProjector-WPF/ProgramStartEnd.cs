using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
        Mutex programMutex;

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

        private void ProgramInit(ProgramStartLoading loadingWindow)
        {
            while (!loadingWindow.onReady) ;

            loadingWindow.setLoadingState("필수 파일 확인중...", 20);
            try
            {
                module.ProgramData.Initialize();
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message, "필수 파일 없음", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                throw new Exception("필수 파일 없음");
            }

            loadingWindow.setLoadingState("필수 파일 확인중...", 30);
            Database.DatabaseInitailize();

            loadingWindow.setLoadingState("PPT 파일 여는중...", 50);
            Powerpoint.Initialize();
            string error = module.ProgramOption.Initialize();
            if (error != null)
                System.Windows.MessageBox.Show
                    (
                    "다음을 확인해주세요 : \r\n" + error,
                    "ppt틀 등록되지 않음",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information
                    );

            loadingWindow.setLoadingState("UI 로딩중...", 65);
            module.ExternPPTManager pptMan = new module.ExternPPTManager();
            module.Data.SongManager songMan = new module.Data.SongManager();
            module.BibleDataManager bibleMan = new module.BibleDataManager();
            module.ReserveDataManager reserveDataManager = new module.ReserveDataManager(bibleMan, songMan, pptMan);

            module.ISearcher searcher = new module.Data.MultiSearcher(
                new module.BibleSearcher(),
                new module.Data.SongSearcher(
                    songMan),
                new module.ExternPPTSearcher(
                    pptMan));

            Event.KeyInputEventManager keyInputEventManager = new Event.KeyInputEventManager();
            Event.WindowActivateChangedEventManager WACEventManager = new Event.WindowActivateChangedEventManager();
            Event.BibleSelectionEventManager bibleSelectionEventManager = new Event.BibleSelectionEventManager();

            loadingWindow.setLoadingState("UI 로딩중...", 80);
            module.ShowStarter showStarter = new module.ShowStarter();

            System.Collections.ObjectModel.Collection<ViewModel.ViewModel> buttonVMs
                = new System.Collections.ObjectModel.Collection<ViewModel.ViewModel>();
            for (int i = 0; i < 6; i++) 
                buttonVMs.Add(new ViewModel.MainPage.VMExternPPTEditButton(reserveDataManager, pptMan, keyInputEventManager, i, showStarter));

            ViewModel.MainPage.VMShowControler[] showControlers = new ViewModel.MainPage.VMShowControler[3];
            showControlers[0] = new ViewModel.MainPage.VMShowControler(ShowContentType.Bible, showStarter, WACEventManager, bibleSelectionEventManager);
            showControlers[1] = new ViewModel.MainPage.VMShowControler(ShowContentType.Song, showStarter, WACEventManager);
            showControlers[2] = new ViewModel.MainPage.VMShowControler(ShowContentType.PPT, showStarter, WACEventManager);

            loadingWindow.setLoadingState("UI 로딩중...", 90);
            module.ProgramData.SaveDataEvent += songMan.saveData_Lyric;
            module.ProgramData.SaveDataEvent += songMan.saveData_Hymn;
            module.ProgramData.SaveDataEvent += pptMan.saveData;
            module.ProgramData.SaveDataEvent += reserveDataManager.saveData;
            module.ProgramData.SaveDataEvent += module.ProgramOption.saveData;

            loadingWindow.setLoadingState("UI 로딩중...", 100);
            ViewModel.VMMainWindow mainViewModel = 
                new ViewModel.VMMainWindow(
                    new ViewModel.MainPage.VMMain(
                        new ViewModel.MainPage.VMControlPage(
                            showControlers,
                            new ViewModel.MainPage.VMBibleSeletion(reserveDataManager, showStarter, bibleSelectionEventManager),
                            new ViewModel.MainPage.VMSearchControl(searcher, reserveDataManager, showStarter, bibleSelectionEventManager),
                            new ViewModel.MainPage.VMReserveList(reserveDataManager, showStarter, bibleSelectionEventManager),
                            buttonVMs,
                            keyInputEventManager,
                            showStarter),
                        new ViewModel.OptionViewModel(),
                        new ViewModel.MainPage.VMOptionBar(),
                        new ViewModel.LyricViewModel(showStarter, songMan, reserveDataManager)),
                    keyInputEventManager,
                    WACEventManager);

            loadingWindow.Dispatcher.BeginInvoke(
                new Action<ViewModel.VMMainWindow>(loadingWindow.InitializeDone),
                mainViewModel);
        }

        public void doProgramInit()
        {
            ProgramStartLoading loadingWindow = new ProgramStartLoading();
            loadingWindow.Show();

            Thread initThread = new Thread((obj) => ProgramInit((ProgramStartLoading)obj));
            initThread.Start(loadingWindow);
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

            System.Windows.MessageBox.Show("프로그램을 종료합니다.");
        }
    }
}
