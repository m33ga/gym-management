using System;
using Windows.UI.Xaml.Data;

namespace GymManagement.UWP.Converters
{
    public class BoolToNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue; // Negates the boolean value
            }
            return false; // Default to false if input is invalid
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return !boolValue; // Negates the boolean value back
            }
            return false;
        }
    }
}
