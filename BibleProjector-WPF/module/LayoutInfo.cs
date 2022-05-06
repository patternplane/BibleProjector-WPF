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

        static public void Initialize()
        {
            string[] data = module.ProgramData.getLayoutData().Split(new string[] { SEPARATOR},StringSplitOptions.None);
            if (data.Length != 20)
                return;

            Layout_MainWindow.Width = double.Parse(data[0]);
            Layout_MainWindow.Height = double.Parse(data[1]);
            Layout_MainWindow.x = double.Parse(data[2]);
            Layout_MainWindow.y = double.Parse(data[3]);

            Layout_BibleControl.Width = double.Parse(data[4]);
            Layout_BibleControl.Height = double.Parse(data[5]);
            Layout_BibleControl.x = double.Parse(data[6]);
            Layout_BibleControl.y = double.Parse(data[7]);

            Layout_ReadingControl.Width = double.Parse(data[8]);
            Layout_ReadingControl.Height = double.Parse(data[9]);
            Layout_ReadingControl.x = double.Parse(data[10]);
            Layout_ReadingControl.y = double.Parse(data[11]);

            Layout_SongControl.Width = double.Parse(data[12]);
            Layout_SongControl.Height = double.Parse(data[13]);
            Layout_SongControl.x = double.Parse(data[14]);
            Layout_SongControl.y = double.Parse(data[15]);

            Layout_ExternPPTControl.Width = double.Parse(data[16]);
            Layout_ExternPPTControl.Height = double.Parse(data[17]);
            Layout_ExternPPTControl.x = double.Parse(data[18]);
            Layout_ExternPPTControl.y = double.Parse(data[19]);
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
