using System;
using System.Globalization;
using System.IO;
using System.Xml;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;

namespace Client.Utility; 

public class SvgPathConverter : IValueConverter {
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is string path && targetType == typeof(Geometry) && parameter is IUriContext context)
        {
            var baseUri = context.BaseUri;
            var data = GetPathDataFromFile(path, baseUri);
            if (data != null)
            {
                return Geometry.Parse(data);
            }
        }

        return new BindingNotification(
            new InvalidCastException($"Could not convert {value} to a {targetType.Name}!"),
            BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
    
    private static string GetPathDataSubString(string data) {
       // get d="..." substring using xml parser
        var xml = new XmlDocument();
        xml.LoadXml(data);
#pragma warning disable CS8602
        var path = xml.GetElementsByTagName("path")[0].Attributes["d"].Value;
#pragma warning restore CS8602
        return path;
    }

    private static string? GetPathDataFromFile(string path, Uri? baseUri) {
        if (File.Exists(path))
        {
            var data = File.ReadAllText(path);
            return GetPathDataSubString(data);
        }

        var uri = path.StartsWith("/")
            ? new Uri(path, UriKind.Relative)
            : new Uri(path, UriKind.RelativeOrAbsolute);
        {
            if (uri is {IsAbsoluteUri: true, IsFile: true})
            {
                var data = File.ReadAllText(uri.LocalPath);
                return GetPathDataSubString(data);
            }
            else
            {
                var loader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                var stream = loader?.Open(uri, baseUri);
                if (stream == null) return null;
                
                using var reader = new StreamReader(stream);
                var data = reader.ReadToEnd();
                return GetPathDataSubString(data);
            }
        }
    }
}