using Microsoft.UI.Xaml.Data;
using System;

namespace Clinica_Vet.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan time)
            {
                return time.ToString(@"hh\:mm");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (TimeSpan.TryParse(value as string, out var time))
            {
                return time;
            }
            return default(TimeSpan);
        }
    }
}
