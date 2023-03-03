namespace Client.ViewModel;

public class MainWindowViewModel : ViewModelBase {
    public MainWindowViewModel() {
        SidebarViewModel = new SidebarViewModel();
        ContentViewModel = new ContentViewModel();
        NavigationViewModel = new NavigationViewModel(SidebarViewModel, ContentViewModel);
    }
    
    public SidebarViewModel SidebarViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
    public NavigationViewModel NavigationViewModel { get; }
}