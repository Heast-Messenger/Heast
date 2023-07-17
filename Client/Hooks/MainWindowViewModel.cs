using System;
using Client.ViewModel;

namespace Client;

public static partial class Hooks
{
    public static Func<MainWindowViewModel> UseMainViewModel()
    {
        var window = UseCurrentWindow();
        return () => (MainWindowViewModel)window().DataContext!;
    }
}