using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client.ViewModel;

namespace Client;

public static partial class Hooks
{
    public static (Func<MainWindowViewModel> Get, Action<MainWindowViewModel> Set) UseMainWindowViewModel()
    {
        Window GetWindow()
        {
            return (Application.Current!.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!.MainWindow!;
        }

        return (
            () => (MainWindowViewModel) GetWindow().DataContext!,
            vm => GetWindow().DataContext = vm
        );
    }
}