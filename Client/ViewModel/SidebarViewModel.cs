using Avalonia.Layout;
using Client.View.Sidebars;

namespace Client.ViewModel;

public class SidebarViewModel : ViewModelBase
{
	private Layoutable _currentSidebar = new EmptySidebar();
	private double _currentSidebarWidth;
	private double _previousSidebarWidth;

	public Layoutable CurrentSidebar
	{
		get => _currentSidebar;
		set
		{
			if (value.GetType() != _currentSidebar.GetType())
			{
				RaiseAndSetIfChanged(ref _currentSidebar, value);
			}

			_previousSidebarWidth = value.Width;
			CurrentSidebarWidth = value.Width;
		}
	}

	public double CurrentSidebarWidth
	{
		get => _currentSidebarWidth;
		set => RaiseAndSetIfChanged(ref _currentSidebarWidth, value);
	}

	public void HideSidebar()
	{
		_previousSidebarWidth = CurrentSidebarWidth;
		CurrentSidebarWidth = 0.0;
	}

	public void ShowSidebar()
	{
		CurrentSidebarWidth = _previousSidebarWidth;
	}

	public void ToggleVisibility()
	{
		if (CurrentSidebarWidth > 0.0)
		{
			HideSidebar();
		}
		else
		{
			ShowSidebar();
		}
	}
}