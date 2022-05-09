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
            if (data.Length != 20)
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
        }

        static public string getSaveData()
        {
            StringBuilder str = new StringBuilder(50);

            str.Append(Layout_MainWindow.Width);
            str.Append(SEPARATOR);
            str.Append(Layout_MainWindow.Height);
            str.Append(SEPARATOR);
            str.Append(Layout_MainWindow.x);
            str.Append(SEPARATOR);
            str.Append(Layout_MainWindow.y);
            str.Append(SEPARATOR);

            str.Append(Layout_BibleControl.Width);
            str.Append(SEPARATOR);
            str.Append(Layout_BibleControl.Height);
            str.Append(SEPARATOR);
            str.Append(Layout_BibleControl.x);
            str.Append(SEPARATOR);
            str.Append(Layout_BibleControl.y);
            str.Append(SEPARATOR);

            str.Append(Layout_ReadingControl.Width);
            str.Append(SEPARATOR);
            str.Append(Layout_ReadingControl.Height);
            str.Append(SEPARATOR);
            str.Append(Layout_ReadingControl.x);
            str.Append(SEPARATOR);
            str.Append(Layout_ReadingControl.y);
            str.Append(SEPARATOR);

            str.Append(Layout_SongControl.Width);
            str.Append(SEPARATOR);
            str.Append(Layout_SongControl.Height);
            str.Append(SEPARATOR);
            str.Append(Layout_SongControl.x);
            str.Append(SEPARATOR);
            str.Append(Layout_SongControl.y);
            str.Append(SEPARATOR);

            str.Append(Layout_ExternPPTControl.Width);
            str.Append(SEPARATOR);
            str.Append(Layout_ExternPPTControl.Height);
            str.Append(SEPARATOR);
            str.Append(Layout_ExternPPTControl.x);
            str.Append(SEPARATOR);
            str.Append(Layout_ExternPPTControl.y);

            return str.ToString();
        }

        static public void removeAllLayoutData()
        {
            Layout_MainWindow.Width = -1;
            Layout_MainWindow.Height = -1;
            Layout_MainWindow.x = -1;
            Layout_MainWindow.y = -1;

            Layout_BibleControl.Width = -1;
            Layout_BibleControl.Height = -1;
            Layout_BibleControl.x = -1;
            Layout_BibleControl.y = -1;

            Layout_ReadingControl.Width = -1;
            Layout_ReadingControl.Height = -1;
            Layout_ReadingControl.x = -1;
            Layout_ReadingControl.y = -1;

            Layout_SongControl.Width = -1;
            Layout_SongControl.Height = -1;
            Layout_SongControl.x = -1;
            Layout_SongControl.y = -1;

            Layout_ExternPPTControl.Width = -1;
            Layout_ExternPPTControl.Height = -1;
            Layout_ExternPPTControl.x = -1;
            Layout_ExternPPTControl.y = -1;
        }
    }
}
