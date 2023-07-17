using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Model;
using Client.ViewModel;

namespace Client.View.Content.Login;

public partial class ConnectPanel : LoginBase
{
    public ConnectPanel()
    {
        InitializeComponent();
    }

    public override LoginBase Back => new ServerOptionsPanel
    {
        DataContext = DataContext
    };

    public override Size? WindowSize => new(400.0, 500.0);

    private LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Control { DataContext: ConnectionStep { Helplink: not null } step })
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = step.Helplink,
                UseShellExecute = true
            });
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Content = new LoginOptionsPanel
        {
            DataContext = DataContext
        };
    }
}