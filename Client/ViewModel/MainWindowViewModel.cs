namespace Client.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public SidebarViewModel SidebarViewModel { get; } = new();
    public ContentViewModel ContentViewModel { get; } = new();
    public NavigationViewModel NavigationViewModel { get; } = new();
    public NotificationsViewModel NotificationsViewModel { get; } = new();
}