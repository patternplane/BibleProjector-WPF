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
            // 프로그램 로딩 창
            ProgramStartLoading startLodingWindow = new ProgramStartLoading();
            startLodingWindow.Show();
            
            Database.DatabaseInitailize();
            module.ProgramOption.Initialize();
            module.LayoutInfo.Initialize();
            
            string error = Powerpoint.Initialize();
            if (error.CompareTo("") != 0)
                System.Windows.MessageBox.Show
                    (
                    "다음을 확인해주세요 : \r\n" + error,
                    "ppt틀 등록되지 않음",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information
                    );

            new MainWindow().Show();

            // 프로그램 로딩 창 종료
            startLodingWindow.Close();
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
