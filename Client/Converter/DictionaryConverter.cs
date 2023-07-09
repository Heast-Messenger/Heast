using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client.Converter;

public class DictionaryConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (parameter is not IDictionary dictionary || value is null)
		{
			return new ArgumentException("Parameter must be of type 'IDictionary'");
		}

		if (!dictionary.Contains(value))
		{
			return new ArgumentException("Value was not found inside the dictionary");
		}

		return dictionary[value];
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}