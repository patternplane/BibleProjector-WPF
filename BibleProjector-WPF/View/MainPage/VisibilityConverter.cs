﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BibleProjector_WPF.View.MainPage
{
    [ValueConversion(typeof(bool), typeof(System.Windows.Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((System.Windows.Visibility)value) == System.Windows.Visibility.Visible;
        }
    }
}
