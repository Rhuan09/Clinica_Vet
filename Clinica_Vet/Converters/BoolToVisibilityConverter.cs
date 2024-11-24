using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Clinica_Vet.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = false;

            if (value is bool b)
            {
                boolValue = b;
            }

            if (Invert)
            {
                boolValue = !boolValue;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility v)
            {
                bool result = v == Visibility.Visible;
                return Invert ? !result : result;
            }
            return false;
        }
    }
}
