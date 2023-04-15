using Avalonia.Layout;
using Client.View.Sidebars;

namespace Client.ViewModel;

public class SidebarViewModel : ViewModelBase
{
    private Layoutable _currentSidebar;
    private double _currentSidebarWidth;

    public SidebarViewModel()
    {
        _currentSidebar = new EmptySidebar();
    }

    public Layoutable CurrentSidebar
    {
        get => _currentSidebar;
        set
        {
            if (value.GetType() == _currentSidebar.GetType())
                return;
            RaiseAndSetIfChanged(ref _currentSidebar, value);
            CurrentSidebarWidth = value.Width;
        }
    }

    public double CurrentSidebarWidth
    {
        get => _currentSidebarWidth;
        set => RaiseAndSetIfChanged(ref _currentSidebarWidth, value);
    }
}