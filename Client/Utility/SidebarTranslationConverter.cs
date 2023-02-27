using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Utility; 

public class SidebarTranslateConverter : IMultiValueConverter {
    public object Convert(IList<object?> value, Type targetType, object? parameter, CultureInfo culture) {
        if (value[0] is double actualWidth && value[1] is double fixedWidth && targetType.IsAssignableFrom(typeof(TranslateTransform))) {
            return new TranslateTransform((fixedWidth - actualWidth) / 2.0, 0);
        }
        
        return new BindingNotification(
            new InvalidCastException($"Could not convert {value} to a {targetType.Name}!"),
            BindingErrorType.Error);
    }
}