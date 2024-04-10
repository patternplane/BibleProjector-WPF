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
                return new SolidColorBrush(BibleDefaultColor);
            else if (type == ShowContentType.Song)
                return new SolidColorBrush(SongDefaultColor);
            else if (type == ShowContentType.PPT)
                return new SolidColorBrush(PPTDefaultColor);
            else
                return Brushes.Gray;
        }

        public static Brush getBrush2(ShowContentType type)
        {
            if (type == ShowContentType.Bible)
                return new SolidColorBrush(BibleBackgroundColor);
            else if (type == ShowContentType.Song)
                return new SolidColorBrush(SongBackgroundColor);
            else if (type == ShowContentType.PPT)
                return new SolidColorBrush(PPTBackgroundColor);
            else
                return Brushes.Gray;
        }

        public static Color BibleDefaultColor { get { return Colors.GreenYellow; } }
        public static Color SongDefaultColor { get { return Colors.Aquamarine; } }
        public static Color PPTDefaultColor { get { return Color.FromArgb(0xFF, 0xFF, 0xC1, 0x83); } }

        public static Color BibleBackgroundColor { get { return Color.FromArgb(0xFF, 0xD2, 0xFF, 0x8C); } }
        public static Color SongBackgroundColor { get { return Color.FromArgb(0xFF, 0xBF, 0xFF, 0xE9); } }
        public static Color PPTBackgroundColor { get { return Color.FromArgb(0xFF, 0xFF, 0xDF, 0xBF); } }
    }
}
