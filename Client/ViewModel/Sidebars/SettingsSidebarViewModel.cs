using System;
using Client.View.Content;
using Client.ViewModel.Content;
using Microsoft.Extensions.DependencyInjection;

namespace Client.ViewModel.Sidebars;

public class SettingsSidebarViewModel : SidebarViewModelBase
{
    public SettingsSidebarViewModel(
        IServiceProvider serviceProvider,
        MainWindowViewModel mainWindowVm) : base(mainWindowVm.ContentViewModel)
    {
        ServiceProvider = serviceProvider;
        Button_OnAccount();
    }

    public SettingsSidebarViewModel()
    {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }

    private IServiceProvider ServiceProvider { get; }

    public void Button_OnAccount()
    {
        ContentViewModel.Content = new SettingsAccountPanel
        {
            DataContext = ServiceProvider.GetService<SettingsAccountViewModel>()
        };
    }

    public void Button_OnSecurity()
    {
        ContentViewModel.Content = new SettingsSecurityPanel
        {
            DataContext = ServiceProvider.GetService<SettingsSecurityViewModel>()
        };
    }

    public void Button_OnNotifications()
    {
        ContentViewModel.Content = new SettingsNotificationsPanel
        {
            DataContext = ServiceProvider.GetService<SettingsNotificationsViewModel>()
        };
    }

    public void Button_OnAppearance()
    {
        ContentViewModel.Content = new SettingsAppearancePanel
        {
            DataContext = ServiceProvider.GetService<SettingsAppearanceViewModel>()
        };
    }

    public void Button_OnStatus()
    {
        ContentViewModel.Content = new SettingsStatusPanel
        {
            DataContext = ServiceProvider.GetService<SettingsStatusViewModel>()
        };
    }
}