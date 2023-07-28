using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Core.Attributes;

namespace Client.Converter;

public class StringValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Enum)
        {
            return value.GetType()
                .GetField(value.ToString()!)!
                .GetCustomAttributes(inherit: false)
                .OfType<StringValueAttribute>()
                .First().Value;
        }

        return new BindingNotification(
            new ArgumentException("value must be of type 'Enum' and targetType of type 'string'"),
            BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}