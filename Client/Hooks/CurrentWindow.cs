using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Client;

public static partial class Hooks
{
    public static Func<Window> UseCurrentWindow()
    {
        return () =>
        {
            var application = Application.Current!;
            var lifetime = application.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            return lifetime!.MainWindow!;
        };
    }
}