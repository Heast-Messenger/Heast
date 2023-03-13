using System;
using Avalonia.Markup.Xaml;

namespace Client.Utility;

public class UriContextGetter : MarkupExtension {
    public override object ProvideValue(IServiceProvider serviceProvider) {
        return (IUriContext) serviceProvider.GetService(typeof(IUriContext))!;
    }
}