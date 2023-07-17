using Avalonia;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View.Content.Login;

public partial class WelcomePanel : LoginBase
{
    public WelcomePanel()
    {
        InitializeComponent();
    }

    public override LoginBase? Back => null;

    public override Size? WindowSize => null;

    private LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindowViewModel.Content = new ServerOptionsPanel
        {
            DataContext = DataContext
        };
    }
}