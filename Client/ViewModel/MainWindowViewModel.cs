using Client.Model;

namespace Client.ViewModel;

public class MainWindowViewModel : ViewModelBase {
    public MainWindowViewModel() {
        SidebarViewModel = new SidebarViewModel();
        Global.Sidebar = SidebarViewModel;
        
        ContentViewModel = new ContentViewModel();
        Global.Content = ContentViewModel;
    }
    
    public SidebarViewModel SidebarViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
}