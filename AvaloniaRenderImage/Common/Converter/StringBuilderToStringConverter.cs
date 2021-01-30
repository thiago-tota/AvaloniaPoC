using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Text;

namespace Common.Converter
{
    public class StringBuilderToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((StringBuilder)value)?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new StringBuilder().AppendLine(value?.ToString());
        }
    }
}
