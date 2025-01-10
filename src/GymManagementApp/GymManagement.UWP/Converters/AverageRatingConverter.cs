using System;
using Windows.UI.Xaml.Data;

public class AverageRatingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is double rating)
        {
            return rating.ToString("F1"); // Format to 1 decimal place
        }
        return "0.0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
