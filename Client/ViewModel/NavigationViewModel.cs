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
        Console.WriteLine("clicked chat");
    }

    public void OnExplore() {
        Console.WriteLine("clicked explore (sidebar hidden)");
        _sidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void OnPeople() {
        Console.WriteLine("clicked people (sidebar shown with friends list)");
    }

    public void OnServers() {
        Console.WriteLine("clicked servers (sidebar shown with server list)");
    }

    public void OnSettings() {
        _sidebarViewModel.CurrentSidebar = new SettingsSidebar
        {
            DataContext = new SettingsSidebarViewModel(_contentViewModel)
        };
    }
}