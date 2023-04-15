using Core.Network.Pipeline;

namespace Client.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(ClientConnection ctx)
    {
        SidebarViewModel = new SidebarViewModel(this);
        ContentViewModel = new ContentViewModel(this);
        NavigationViewModel = new NavigationViewModel(this);
        Ctx = ctx;
    }

    public SidebarViewModel SidebarViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
    public NavigationViewModel NavigationViewModel { get; }

    public ClientConnection Ctx { get; set; }
}