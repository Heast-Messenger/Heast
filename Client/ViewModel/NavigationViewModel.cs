using System;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public NavigationViewModel(MainWindowViewModel mainWindowVm)
    {
        _mainWindowVm = mainWindowVm;
    }

    public NavigationViewModel()
    {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }

    public void Button_OnChat()
    {
        _mainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnExplore()
    {
        _mainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnPeople()
    {
        _mainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnServers()
    {
        _mainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnSettings()
    {
        _mainWindowVm.SidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = new SettingsSidebarViewModel(_mainWindowVm)
        };
    }
}