using System;
using Avalonia.Layout;
using Client.View.Sidebars;
using Client.ViewModel.Sidebars;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase {
    public NavigationViewModel(SidebarViewModel sidebarViewModel, ContentViewModel contentViewModel) {
        SidebarViewModel = sidebarViewModel;
        ContentViewModel = contentViewModel;
    }
    
    public NavigationViewModel() {
        throw new InvalidOperationException("This constructor is only for design-time purposes.");
    }
    
    public readonly SidebarViewModel SidebarViewModel;
    public readonly ContentViewModel ContentViewModel;
}