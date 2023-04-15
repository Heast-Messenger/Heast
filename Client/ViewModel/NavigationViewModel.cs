using System;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    public readonly ContentViewModel ContentViewModel;

    public readonly SidebarViewModel SidebarViewModel;

    public NavigationViewModel(SidebarViewModel sidebarViewModel, ContentViewModel contentViewModel)
    {
        SidebarViewModel = sidebarViewModel;
        ContentViewModel = contentViewModel;
    }

    public NavigationViewModel()
    {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }

    public void Button_OnChat()
    {
        SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnExplore()
    {
        SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnPeople()
    {
        SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnServers()
    {
        SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnSettings()
    {
        SidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = new SettingsSidebarViewModel(ContentViewModel)
        };
    }
}