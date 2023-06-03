using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace Client.Converter;

public class OrientationConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is bool val)
		{
			return val ? Orientation.Horizontal : Orientation.Vertical;
		}

		throw new ArgumentException("Value is not a boolean", nameof(value));
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}