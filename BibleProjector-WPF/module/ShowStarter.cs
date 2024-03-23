using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BibleProjector_WPF.module
{
    public class ShowStarter
    {
        public Event.ShowStartEventHandler ShowStartEvent;

        public void Show(Data.ShowData showData)
        {
            if (showData.canExcuteShow() == module.Data.ShowExcuteErrorEnum.NoneFrameFile)
                MessageBox.Show("틀 파일이 입력되지 않음");
            else if (showData.canExcuteShow() == module.Data.ShowExcuteErrorEnum.InvalidData)
                MessageBox.Show("주어진 자료는 잘못된 자료입니다!");
            else
            {
                if (showData.canExcuteShow() != Data.ShowExcuteErrorEnum.NoErrors)
                    throw new Exception("슬라이드 쇼를 실행할 수 없는 항목을 표시하려 했습니다. " + showData.canExcuteShow().ToString());
                else
                    ShowStartEvent.Invoke(this, new Event.ShowStartEventArgs(showData));
            }
        }
    }
}
