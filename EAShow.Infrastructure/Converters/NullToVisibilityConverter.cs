using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace EAShow.Infrastructure.Converters
{
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool.TryParse(value: (string) parameter, out var flip);

            return value == null ? flip ? Visibility.Visible : Visibility.Collapsed : flip ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
