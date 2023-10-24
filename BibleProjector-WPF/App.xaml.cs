using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BibleProjector_WPF
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {

        System.Threading.Mutex mutex;
        public const string PROGRAM_FULLNAME = "WorshipProjector-WPF-BSS";

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew = false;
            mutex = new System.Threading.Mutex(true, PROGRAM_FULLNAME, out createdNew);

            if (createdNew)
            {
                base.OnStartup(e);
            }
            else
            {
                System.Windows.MessageBox.Show("프로그램은 한번만 실행하세요!", "프로그램이 이미 작동중", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }

        // 테스트로 넘기는 기능
        void Startup_test(object sender,StartupEventArgs e)
        {
            UnitTester.Tester.TestStart();
        }

        void ProgramExit(object sender, ExitEventArgs e)
        {
            Console.WriteLine("프로그램 정상 종료");
        }

        void ExitFromError(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine("프로그램 에러 종료");

            module.ProgramData.saveProgramData();
            Powerpoint.FinallProcess();
            
            MessageBox.Show("프로그램 결함으로 강제 종료합니다.");
        }
    }
}
