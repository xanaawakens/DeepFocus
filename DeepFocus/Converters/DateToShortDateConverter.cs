using Microsoft.UI.Xaml.Data;
using System;

namespace DeepFocus.Converters
{
    public sealed class DateToShortDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return date.ToString("d");
            }
            if (value is DateTimeOffset dateOffset)
            {
                return dateOffset.ToString("d");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateString && DateTime.TryParse(dateString, out DateTime result))
            {
                return result;
            }
            return DateTime.MinValue;
        }
    }
}
