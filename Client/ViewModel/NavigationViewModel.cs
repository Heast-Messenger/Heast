using System;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;
using static Client.Hooks;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase
{
    private Func<MainWindowViewModel> MainWindow => UseMainViewModel();

    public void Button_OnNav()
    {
        MainWindow().SidebarViewModel.ToggleVisibility();
    }

    public void Button_OnHome()
    {
        MainWindow().SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }


    public void Button_OnChat()
    {
        MainWindow().SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnExplore()
    {
        MainWindow().SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnPeople()
    {
        MainWindow().SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnServers()
    {
        MainWindow().SidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void Button_OnSettings()
    {
        MainWindow().SidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = new SettingsSidebarViewModel(MainWindow())
        };
    }
}