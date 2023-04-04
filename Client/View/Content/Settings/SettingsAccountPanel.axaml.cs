using Avalonia.Interactivity;
using Client.ViewModel.Content;

namespace Client.View.Content; 

public partial class SettingsAccountPanel : ContentBase {
    public SettingsAccountPanel() {
        InitializeComponent();
    }

    private SettingsAccountViewModel ViewModel =>
        (DataContext as SettingsAccountViewModel)!;

    private void Button_OnInvite(object? sender, RoutedEventArgs e) {
        ViewModel.Invite();
    }

    private void Button_OnRequest(object? sender, RoutedEventArgs e) {
        ViewModel.Request();
    }

    private void Button_OnSwitch(object? sender, RoutedEventArgs e) {
        ViewModel.Switch();
    }

    private void Button_OnLogout(object? sender, RoutedEventArgs e) {
        ViewModel.Logout();
    }

    private void Button_OnDelete(object? sender, RoutedEventArgs e) {
        ViewModel.Delete();
    }
}
