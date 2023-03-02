using System;
using Avalonia.Layout;
using Client.View.Sidebars;

namespace Client.ViewModel; 

public class SidebarViewModel : ViewModelBase {
    public SidebarViewModel() {
        _currentSidebar = new EmptySidebar();
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