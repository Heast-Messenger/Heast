using Client.View.Sidebars;
using Client.ViewModel.Sidebars;
using static Client.Hooks;

namespace Client.ViewModel;

public class NavigationViewModel : ViewModelBase
{
	private MainWindowViewModel MainWindow => UseMainWindow();

	public void Button_OnChat()
	{
		this.MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
	}

	public void Button_OnExplore()
	{
		this.MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
	}

	public void Button_OnPeople()
	{
		this.MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
	}

	public void Button_OnServers()
	{
		this.MainWindow.SidebarViewModel.CurrentSidebar = new EmptySidebar();
	}

	public void Button_OnSettings()
	{
		this.MainWindow.SidebarViewModel.CurrentSidebar = new SettingsSidebar
		{
			DataContext = new SettingsSidebarViewModel(this.MainWindow)
		};
	}
}