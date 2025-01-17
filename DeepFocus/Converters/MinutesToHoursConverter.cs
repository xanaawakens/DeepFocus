using Microsoft.UI.Xaml.Data;
using System;

namespace DeepFocus.Converters
{
    public class MinutesToHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int minutes)
            {
                double hours = Math.Round(minutes / 60.0, 1);
                return $"{hours:F1}h";
            }
            return "0h";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
