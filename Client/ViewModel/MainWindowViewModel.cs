using System;
using Microsoft.Extensions.DependencyInjection;

namespace Client.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public SidebarViewModel SidebarViewModel => ServiceProvider.GetService<SidebarViewModel>();
    public ContentViewModel ContentViewModel => ServiceProvider.GetService<ContentViewModel>();
    public NavigationViewModel NavigationViewModel => ServiceProvider.GetService<NavigationViewModel>();
    public NotificationsViewModel NotificationsViewModel => ServiceProvider.GetService<NotificationsViewModel>();
}