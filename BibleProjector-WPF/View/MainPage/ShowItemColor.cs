using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BibleProjector_WPF.View.MainPage
{
    class ShowItemColor
    {
        public static Brush getBrush(ShowContentType type)
        {
            if (type == ShowContentType.Bible)
                return new SolidColorBrush(BibleDefaultBrush);
            else if (type == ShowContentType.Song)
                return new SolidColorBrush(SongDefaultBrush);
            else if (type == ShowContentType.PPT)
                return new SolidColorBrush(PPTDefaultBrush);
            else
                return Brushes.Gray;
        }

        public static Brush getBrush2(ShowContentType type)
        {
            if (type == ShowContentType.Bible)
                return new SolidColorBrush(BibleBackgroundBrush);
            else if (type == ShowContentType.Song)
                return new SolidColorBrush(SongBackgroundBrush);
            else if (type == ShowContentType.PPT)
                return new SolidColorBrush(PPTBackgroundBrush);
            else
                return Brushes.Gray;
        }

        public static Color BibleDefaultBrush { get { return Colors.GreenYellow; } }
        public static Color SongDefaultBrush { get { return Colors.Aquamarine; } }
        public static Color PPTDefaultBrush { get { return Color.FromArgb(0xFF, 0xFF, 0xC1, 0x83); } }

        public static Color BibleBackgroundBrush { get { return Color.FromArgb(0xFF, 0xE6, 0xFF, 0xC0); } }
        public static Color SongBackgroundBrush { get { return Color.FromArgb(0xFF, 0xD8, 0xFF, 0xF2); } }
        public static Color PPTBackgroundBrush { get { return Color.FromArgb(0xFF, 0xFF, 0xEC, 0xDA); } }
    }
}
