using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace BibleProjector_WPF.module
{
    class ProgramData
    {
        // 성경 예약
        static public BindingList<ViewModel.BibleReserveData.BibleReserveContent> BibleReserveList;

        // =========================================== 프로그램 초기 세팅 =========================================== 

        static public void getProgramData()
        {
            getBibleReserveData();
        }

        static void getBibleReserveData()
        {
            BibleReserveList = new BindingList<ViewModel.BibleReserveData.BibleReserveContent>();
        }

        // =========================================== 프로그램 종료 ===========================================

        static public void saveProgramData()
        {

        }
    }
}
