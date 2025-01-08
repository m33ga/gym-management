﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace GymManagement.UWP.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (value is Visibility visibility && visibility == Visibility.Visible);
        }
    }
}