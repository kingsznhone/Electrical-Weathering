using System;
using System.Globalization;
using System.Windows.Data;

namespace Electrical_Weathering
{
    [ValueConversion(typeof(WeatheringMode), typeof(bool))]
    public class ModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = (WeatheringMode)value == WeatheringMode.SKIA ? true : false;
            return !result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value) return WeatheringMode.OPENCV;
            else return WeatheringMode.SKIA;
        }
    }
}