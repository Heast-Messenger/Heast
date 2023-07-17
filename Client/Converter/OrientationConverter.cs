using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace Client.Converter;

public class OrientationConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool val && targetType == typeof(Orientation))
        {
            return val ? Orientation.Horizontal : Orientation.Vertical;
        }

        return new BindingNotification(
            new ArgumentException("value must be of type 'bool' and targetType of type 'Orientation'"),
            BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}