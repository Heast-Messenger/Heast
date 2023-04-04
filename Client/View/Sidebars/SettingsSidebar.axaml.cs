using Avalonia.Interactivity;
using Client.View.Content;
using Client.ViewModel.Content;
using Client.ViewModel.Sidebars;

namespace Client.View.Sidebars; 

public partial class SettingsSidebar : SidebarBase {
    public SettingsSidebar() {
        InitializeComponent();
    }
    
    private SettingsSidebarViewModel ViewModel => 
        (DataContext as SettingsSidebarViewModel)!;

    private void Button_OnAccount(object? sender, RoutedEventArgs e) {
        ViewModel.ContentViewModel.Content = new SettingsAccountPanel
        {
            DataContext = new SettingsAccountViewModel()
        };
    }

    private void Button_OnSecurity(object? sender, RoutedEventArgs e) {
        ViewModel.ContentViewModel.Content = new SettingsSecurityPanel
        {
            DataContext = new SettingsSecurityViewModel()
        };
    }

    private void Button_OnNotifications(object? sender, RoutedEventArgs e) {
        ViewModel.ContentViewModel.Content = new SettingsNotificationsPanel
        {
            DataContext = new SettingsNotificationsViewModel()
        };
    }

    private void Button_OnAppearance(object? sender, RoutedEventArgs e) {
        ViewModel.ContentViewModel.Content = new SettingsAppearancePanel
        {
            DataContext = new SettingsAppearanceViewModel()
        };
    }

    private void Button_OnStatus(object? sender, RoutedEventArgs e) {
        ViewModel.ContentViewModel.Content = new SettingsStatusPanel
        {
            DataContext = new SettingsStatusViewModel()
        };
    }
}