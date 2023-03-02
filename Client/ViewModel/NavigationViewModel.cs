using System;
using Client.View.Sidebars;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase {
    public NavigationViewModel(SidebarViewModel sidebarViewModel) {
        _sidebarViewModel = sidebarViewModel;
    }
    
    public NavigationViewModel() {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
    
    private readonly SidebarViewModel _sidebarViewModel;

    public void OnChat() {
        Console.WriteLine("clicked chat");
    }

    public void OnExplore() {
        Console.WriteLine("clicked explore (sidebar hidden)");
        _sidebarViewModel.CurrentSidebar = new EmptySidebar();
    }

    public void OnPeople() {
        Console.WriteLine("clicked people (sidebar shown with friends list)");
        _sidebarViewModel.CurrentSidebar = new FriendList();
    }

    public void OnServers() {
        Console.WriteLine("clicked servers (sidebar shown with server list)");
        _sidebarViewModel.CurrentSidebar = new ServerList();
    }

    public void OnSettings() {
        Console.WriteLine("clicked settings");
    }
}