using System;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;
using static Client.Hooks;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    private readonly (Func<MainWindowViewModel> Get, Action<MainWindowViewModel> Set) _mainWindowVm =
        UseMainWindowViewModel();

    private MainWindowViewModel MainWindowVm => _mainWindowVm.Get();

    public void Button_OnChat()
    {
        MainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnExplore()
    {
        MainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnPeople()
    {
        MainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnServers()
    {
        MainWindowVm.SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnSettings()
    {
        MainWindowVm.SidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = new SettingsSidebarViewModel(MainWindowVm)
        };
    }
}