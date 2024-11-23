using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace Clinica_Vet.Converters
{
    public class IdToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int id)
            {
                return id == 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
