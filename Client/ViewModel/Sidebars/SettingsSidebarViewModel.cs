using System;
using Client.View.Content;
using Client.ViewModel.Content;

namespace Client.ViewModel.Sidebars;

public class SettingsSidebarViewModel : SidebarViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public SettingsSidebarViewModel(MainWindowViewModel mainWindowVm) : base(mainWindowVm.ContentViewModel)
    {
        _mainWindowVm = mainWindowVm;
        ContentViewModel.Content = new SettingsAccountPanel
        {
            DataContext = new SettingsAccountViewModel(_mainWindowVm)
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
            DataContext = new SettingsAccountViewModel(_mainWindowVm)
        };
    }

    public void Button_OnSecurity()
    {
        ContentViewModel.Content = new SettingsSecurityPanel
        {
            DataContext = new SettingsSecurityViewModel(_mainWindowVm)
        };
    }

    public void Button_OnNotifications()
    {
        ContentViewModel.Content = new SettingsNotificationsPanel
        {
            DataContext = new SettingsNotificationsViewModel(_mainWindowVm)
        };
    }

    public void Button_OnAppearance()
    {
        ContentViewModel.Content = new SettingsAppearancePanel
        {
            DataContext = new SettingsAppearanceViewModel(_mainWindowVm)
        };
    }

    public void Button_OnStatus()
    {
        ContentViewModel.Content = new SettingsStatusPanel
        {
            DataContext = new SettingsStatusViewModel(_mainWindowVm)
        };
    }
}