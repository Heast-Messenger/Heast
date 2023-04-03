using Avalonia.Interactivity;
using Client.View.Content;
using Client.ViewModel.Sidebars;

namespace Client.View.Sidebars; 

public partial class SettingsSidebar : SidebarBase {
    public SettingsSidebar() {
        InitializeComponent();
    }
    
    private SettingsSidebarViewModel ViewModel => 
        (DataContext as SettingsSidebarViewModel)!;

    private void Button_OnAccount(object? sender, RoutedEventArgs e) {
        ViewModel.NavigateTo(new SettingsAccountPanel());
    }

    private void Button_OnSecurity(object? sender, RoutedEventArgs e) {
        ViewModel.NavigateTo(new SettingsSecurityPanel());
    }

    private void Button_OnNotifications(object? sender, RoutedEventArgs e) {
        ViewModel.NavigateTo(new SettingsNotificationsPanel());
    }

    private void Button_OnAppearance(object? sender, RoutedEventArgs e) {
        ViewModel.NavigateTo(new SettingsAppearancePanel());
    }

    private void Button_OnStatus(object? sender, RoutedEventArgs e) {
        ViewModel.NavigateTo(new SettingsStatusPanel());
    }
}