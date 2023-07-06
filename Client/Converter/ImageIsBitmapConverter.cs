using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Client.Converter;

public class ImageIsBitmapConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string s && targetType == typeof(bool))
		{
			return s.EndsWith(".png") || s.EndsWith(".jpg");
		}

		return new BindingNotification(
			new ArgumentException("value must be of type 'string' and targetType of type 'bool'"),
			BindingErrorType.Error);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}