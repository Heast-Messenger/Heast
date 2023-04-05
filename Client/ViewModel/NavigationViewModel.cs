using System;

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
}