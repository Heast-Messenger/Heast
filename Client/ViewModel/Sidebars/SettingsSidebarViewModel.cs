using System;
using Client.View.Content;
using Client.ViewModel.Content;

namespace Client.ViewModel.Sidebars;

public class SettingsSidebarViewModel : SidebarViewModelBase
{
    public SettingsSidebarViewModel(ContentViewModel contentViewModel) : base(contentViewModel)
    {
        ContentViewModel.Content = new SettingsAccountPanel
        {
            DataContext = new SettingsAccountViewModel()
        };
    }

    public SettingsSidebarViewModel()
    {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }

    public void Button_OnAccount()
    {
        ContentViewModel.Content = new SettingsAccountPanel
        {
            DataContext = new SettingsAccountViewModel()
        };
    }

    public void Button_OnSecurity()
    {
        ContentViewModel.Content = new SettingsSecurityPanel
        {
            DataContext = new SettingsSecurityViewModel()
        };
    }

    public void Button_OnNotifications()
    {
        ContentViewModel.Content = new SettingsNotificationsPanel
        {
            DataContext = new SettingsNotificationsViewModel()
        };
    }

    public void Button_OnAppearance()
    {
        ContentViewModel.Content = new SettingsAppearancePanel
        {
            DataContext = new SettingsAppearanceViewModel()
        };
    }

    public void Button_OnStatus()
    {
        ContentViewModel.Content = new SettingsStatusPanel
        {
            DataContext = new SettingsStatusViewModel()
        };
    }
}