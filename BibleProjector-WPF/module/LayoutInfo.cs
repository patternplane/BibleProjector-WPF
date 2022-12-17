using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleProjector_WPF.module
{
    class LayoutInfo
    {
        const string SEPARATOR = "∂";

        public class SingleLayoutData
        {
            public double Width;
            public double Height;
            public double x;
            public double y;
        }

        static public SingleLayoutData Layout_MainWindow = new SingleLayoutData { Width = -1, Height = -1, x = 0, y = 0 };
        static public SingleLayoutData Layout_BibleControl = new SingleLayoutData { Width = -1, Height = -1, x = 0, y = 0 };
        static public SingleLayoutData Layout_ReadingControl = new SingleLayoutData { Width = -1, Height = -1, x = 0, y = 0 };
        static public SingleLayoutData Layout_SongControl = new SingleLayoutData { Width = -1, Height = -1, x = 0, y = 0 };
        static public SingleLayoutData Layout_ExternPPTControl = new SingleLayoutData { Width = -1, Height = -1, x = 0, y = 0 };
        static public SingleLayoutData Layout_ReserveWindow = new SingleLayoutData { Width = -1, Height = -1, x = 0, y = 0 };

        static void SingleInitializer(SingleLayoutData layout, double width, double height, double x, double y)
        {
            if (width < 0 || height < 0)
            {
                layout.Width = -1;
                layout.Height = -1;
                layout.x = 0;
                layout.y = 0;
            }
            else {
                layout.Width = width;
                layout.Height = height;

                if (x < 0)
                    layout.x = 0;
                else if (x + width > System.Windows.SystemParameters.PrimaryScreenWidth)
                    layout.x = System.Windows.SystemParameters.PrimaryScreenWidth - width;
                else
                    layout.x = x;

                if (y < 0)
                    layout.y = 0;
                else if (y + height > System.Windows.SystemParameters.PrimaryScreenHeight)
                    layout.y = System.Windows.SystemParameters.PrimaryScreenHeight - height;
                else
                    layout.y = y;
            }
        }

        static public void Initialize()
        {
            string[] data = module.ProgramData.getLayoutData().Split(new string[] { SEPARATOR},StringSplitOptions.None);
            if (data.Length < 20)
                return;

            SingleInitializer(
                Layout_MainWindow
                , double.Parse(data[0])
                , double.Parse(data[1])
                , double.Parse(data[2])
                , double.Parse(data[3]));

            SingleInitializer(
                Layout_BibleControl
                , double.Parse(data[4])
                , double.Parse(data[5])
                , double.Parse(data[6])
                , double.Parse(data[7]));

            SingleInitializer(
                Layout_ReadingControl
                , double.Parse(data[8])
                , double.Parse(data[9])
                , double.Parse(data[10])
                , double.Parse(data[11]));

            SingleInitializer(
                Layout_SongControl
                , double.Parse(data[12])
                , double.Parse(data[13])
                , double.Parse(data[14])
                , double.Parse(data[15]));

            SingleInitializer(
                Layout_ExternPPTControl
                , double.Parse(data[16])
                , double.Parse(data[17])
                , double.Parse(data[18])
                , double.Parse(data[19]));

            // 예약창 생긴 이후로 추가된 부분
            if (data.Length > 20)
                SingleInitializer(
                Layout_ReserveWindow
                , double.Parse(data[20])
                , double.Parse(data[21])
                , double.Parse(data[22])
                , double.Parse(data[23]));
        }

        static void layoutStringAppender(StringBuilder str, SingleLayoutData layout)
        {
            str.Append(layout.Width);
            str.Append(SEPARATOR);
            str.Append(layout.Height);
            str.Append(SEPARATOR);
            str.Append(layout.x);
            str.Append(SEPARATOR);
            str.Append(layout.y);
        }
        static public string getSaveData()
        {
            StringBuilder str = new StringBuilder(50);

            layoutStringAppender(str,Layout_MainWindow);
            str.Append(SEPARATOR);

            layoutStringAppender(str, Layout_BibleControl);
            str.Append(SEPARATOR);

            layoutStringAppender(str, Layout_ReadingControl);
            str.Append(SEPARATOR);

            layoutStringAppender(str, Layout_SongControl);
            str.Append(SEPARATOR);

            layoutStringAppender(str, Layout_ExternPPTControl);
            str.Append(SEPARATOR);

            layoutStringAppender(str, Layout_ReserveWindow);

            return str.ToString();
        }

        static void removeLayoutData(SingleLayoutData layout)
        {
            layout.Width = -1;
            layout.Height = -1;
            layout.x = -1;
            layout.y = -1;
        }
        static public void removeAllLayoutData()
        {
            removeLayoutData(Layout_MainWindow);
            removeLayoutData(Layout_BibleControl);
            removeLayoutData(Layout_ReadingControl);
            removeLayoutData(Layout_SongControl);
            removeLayoutData(Layout_ExternPPTControl);
            removeLayoutData(Layout_ReserveWindow);
        }
    }
}
