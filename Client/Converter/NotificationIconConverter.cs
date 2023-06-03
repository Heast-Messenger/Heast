using System;
using System.Globalization;
using Avalonia.Controls.Notifications;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Converter;

public class NotificationIconConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not NotificationType type || targetType != typeof(IImage))
			throw new ArgumentException();

		return type switch
		{
			// NotificationType.Information => new SvgSource() { Source = "/Assets/Notifications/Close.svg }",
			// NotificationType.Warning => "new SvgImage() { Source = /Assets/Notifications/Close.svg" },
			// NotificationType.Error => new SvgImage() { Source = "/Assets/Notifications/Close.svg }",
			// NotificationType.Success => new SvgImage() { Source = "/Assets/Notifications/Close.svg }",
			_ => ""
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}