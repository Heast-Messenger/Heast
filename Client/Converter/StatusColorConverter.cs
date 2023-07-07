using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Client.Model;

namespace Client.Converter;

public class StatusColorConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not StatusType type || targetType != typeof(IBrush))
		{
			return new BindingNotification(
				new ArgumentException("value must be of type 'StatusType' and targetType of type 'IBrush'"),
				BindingErrorType.Error);
		}

		// return type switch
		// {
		// 	StatusType.Pending => Brush.Parse("#4C708B"),
		// 	StatusType.Slow => Brush.Parse("#7E6032"),
		// 	StatusType.Closed => Brush.Parse("#7E3232"),
		// 	StatusType.Running => Brush.Parse("#377639"),
		// 	_ => Brush.Parse("#434343")
		// };

		return type switch
		{
			StatusType.Pending => Brush.Parse("#FF908F97"),
			StatusType.Slow => Brush.Parse("#FFE86F58"),
			StatusType.Closed => Brush.Parse("#FF5CC681"),
			StatusType.Running => Brush.Parse("#FFE9A66B"),
			_ => throw new ArgumentException("Argument out of range.")
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}