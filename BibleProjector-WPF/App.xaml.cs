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
        // ===================== 테스트 처리 ====================

        void Startup_test(object sender, StartupEventArgs e)
        {
            UnitTester.Tester.TestStart();

            Application.Current.Shutdown();
        }

        // ===================== 프로그램 시작 처리 ====================

        void ProgramStart(object sender, StartupEventArgs e)
        {
            ProgramStartEnd.getProgramStartEnd().doProgramInit();
        }

        // ===================== 프로그램 종료 처리 ====================

        void ProgramExit(object sender, ExitEventArgs e)
        {
            ProgramStartEnd.getProgramStartEnd().doPostProcess_byNonError();
        }

        private static bool errorInvoked = false;
        private static bool ignoreAdditionError = false;

        void ExitFromError(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (ignoreAdditionError)
                return;

            if (errorInvoked)
            {
                ignoreAdditionError = true;
                module.ProgramData.writeErrorLog("● 프로그램 종료 진행 중 에러 (Thread 에러 등의 다른 원인에 의한 에러 중복)", e.Exception);
            }
            else
            {
                errorInvoked = true;
                module.ProgramData.writeErrorLog(null, e.Exception);
                ProgramStartEnd.getProgramStartEnd().doPostProcess_byError();
            }
        }
    }
}
