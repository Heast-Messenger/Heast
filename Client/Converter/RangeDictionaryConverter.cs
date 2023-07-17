using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client.Converter;

public class RangeDictionaryConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not IDictionary dictionary || value is null)
        {
            return new ArgumentException("Parameter must be of type 'IDictionary'");
        }

        object? target = null;
        foreach (DictionaryEntry entry in dictionary)
        {
            if ((long)value >= (long)entry.Key)
            {
                target = entry.Value!;
            }
        }

        if (target is null)
        {
            return new ArgumentException("Value was not found inside the dictionary");
        }

        return target;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}