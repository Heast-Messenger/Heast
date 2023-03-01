using Avalonia.Layout;
using Client.View.Sidebars;

namespace Client.ViewModel; 

public class SidebarViewModel : ViewModelBase {
    
    public SidebarViewModel() {
        _isOpened = false;
        _currentSidebar = new EmptySidebar();
    }
    
    private bool _isOpened;
    public bool IsOpened {
        get => _isOpened;
        set => RaiseAndSetIfChanged(ref _isOpened, value);
    }
    
    private Layoutable _currentSidebar;
    public Layoutable CurrentSidebar {
        get => _currentSidebar;
        set {
            RaiseAndSetIfChanged(ref _currentSidebar, value);
            CurrentSidebarWidth = value.Width;
        }
    }

    private double _currentSidebarWidth;
    public double CurrentSidebarWidth {
        get => _currentSidebarWidth;
        set => RaiseAndSetIfChanged(ref _currentSidebarWidth, value);
    }
}