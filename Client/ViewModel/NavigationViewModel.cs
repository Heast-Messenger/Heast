using System;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;
using Microsoft.Extensions.DependencyInjection;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    public NavigationViewModel(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    private MainWindowViewModel MainWindow => ServiceProvider.GetService<MainWindowViewModel>();

    public void Button_OnNav()
    {
        MainWindow.SidebarViewModel.ToggleVisibility();
    }

    public void Button_OnHome()
    {
        MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }


    public void Button_OnChat()
    {
        MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnExplore()
    {
        MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnPeople()
    {
        MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnServers()
    {
        MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnSettings()
    {
        MainWindow.SidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = ServiceProvider.GetService<SettingsSidebarViewModel>()
        };
    }
}