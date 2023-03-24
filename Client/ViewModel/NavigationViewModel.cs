using System;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase {
    public NavigationViewModel(SidebarViewModel sidebarViewModel, ContentViewModel contentViewModel) {
        _sidebarViewModel = sidebarViewModel;
        _contentViewModel = contentViewModel;
    }
    
    public NavigationViewModel() {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
    
    private readonly SidebarViewModel _sidebarViewModel;
    private readonly ContentViewModel _contentViewModel;

    public void OnChat() {
        _sidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void OnExplore() {
        _sidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void OnPeople() {
        _sidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void OnServers() {
        _sidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void OnSettings() {
        _sidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = new SettingsSidebarViewModel(_contentViewModel)
        };
    }
}