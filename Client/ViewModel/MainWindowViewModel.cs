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

    public SidebarViewModel SidebarViewModel => ServiceProvider.GetRequiredService<SidebarViewModel>();
    public ContentViewModel ContentViewModel => ServiceProvider.GetRequiredService<ContentViewModel>();
    public NavigationViewModel NavigationViewModel => ServiceProvider.GetRequiredService<NavigationViewModel>();
    public NotificationsViewModel NotificationsViewModel => ServiceProvider.GetRequiredService<NotificationsViewModel>();
}