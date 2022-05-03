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
        void ProgramExit(object sender, ExitEventArgs e)
        {
            Console.WriteLine("프로그램 정상 종료");
        }

        void ExitFromError(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine("프로그램 에러 종료");
            
            Powerpoint.FinallProcess();
            
            MessageBox.Show("프로그램 결함으로 강제 종료합니다.");
        }
    }
}
