using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Client.Converter;

public class ImageLoaderConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string path && targetType == typeof(IImage))
		{
			try
			{
				var uri = new Uri(path);
				var asset = AssetLoader.Open(uri);
				return new Bitmap(asset);
			}
			catch (Exception)
			{
				// ignored
			}
		}

		return new BindingNotification(
			new ArgumentException("value must be of type 'string' and targetType of type 'IImage'"),
			BindingErrorType.Error);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}