using Microsoft.UI.Xaml.Data;
using System;

namespace DeepFocus.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is not string format)
                return value?.ToString() ?? string.Empty;

            if (value == null)
                return string.Empty;

            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
