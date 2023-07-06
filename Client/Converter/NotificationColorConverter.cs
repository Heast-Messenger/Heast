using System;
using System.Globalization;
using Avalonia.Controls.Notifications;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Converter;

public class NotificationColorConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not NotificationType type || targetType != typeof(IBrush))
		{
			return new BindingNotification(
				new ArgumentException("value must be of type 'NotificationType' and targetType of type 'IBrush'"),
				BindingErrorType.Error);
		}

		return type switch
		{
			NotificationType.Information => Brush.Parse("#4C708B"),
			NotificationType.Warning => Brush.Parse("#7E6032"),
			NotificationType.Error => Brush.Parse("#7E3232"),
			NotificationType.Success => Brush.Parse("#377639"),
			_ => Brush.Parse("#434343")
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}