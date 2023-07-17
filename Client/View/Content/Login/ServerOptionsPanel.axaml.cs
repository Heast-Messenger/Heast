using Avalonia;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View.Content.Login;

public partial class ServerOptionsPanel : LoginBase
{
    public ServerOptionsPanel()
    {
        InitializeComponent();
    }

    public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

    public override LoginBase Back => new WelcomePanel
    {
        DataContext = DataContext
    };

    public override Size? WindowSize => null;

    private void Button_OnOfficial(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.ConnectOfficial();
    }

    private void Button_OnCustom(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Content = new CustomServerPanel
        {
            DataContext = DataContext
        };
    }
}