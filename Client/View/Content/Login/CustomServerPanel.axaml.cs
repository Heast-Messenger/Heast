using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Model;
using Client.ViewModel;

namespace Client.View.Content.Login;

public partial class CustomServerPanel : LoginBase
{
    public CustomServerPanel()
    {
        InitializeComponent();
    }

    public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

    public override LoginBase Back => new ServerOptionsPanel
    {
        DataContext = DataContext
    };

    private void Button_OnConnect(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.ConnectCustom();
    }

    private void Button_OnAdd(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.AddServer();
    }

    private void CustomServer_OnClick(object? sender, PointerPressedEventArgs e)
    {
        if (e.ClickCount == 1)
        {
            var server = ((sender as Control)!.DataContext as CustomServer)!;
            LoginWindowViewModel.CustomServerAddress = server.Address;
        }

        if (e.ClickCount == 2)
        {
            LoginWindowViewModel.ConnectCustom();
        }
    }
}