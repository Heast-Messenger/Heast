using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Client.Model;
using Client.View.Content;
using Client.ViewModel.Content;

namespace Client.ViewModel.Sidebars; 

public class SettingsSidebarViewModel : SidebarViewModelBase {
	public SettingsSidebarViewModel(ContentViewModel contentViewModel) : base(contentViewModel) {
		NavigateTo(SidebarItems[0]);
	}

	public SettingsSidebarViewModel() {
		throw new InvalidOperationException("This constructor is only for design-time purposes.");
	}
	
	public ObservableCollection<SettingsSidebarItem> SidebarItems { get; } = new() {
		new("/Assets/Settings/Account.svg", "Account", null!),
		new("/Assets/Settings/Security.svg", "Security", null!),
		new("/Assets/Settings/Notifications.svg", "Notifications", null!),
		new("/Assets/Settings/Appearance.svg", "Appearance", null!),
		new("/Assets/Settings/Status.svg", "Status", new SettingsStatusPanel {
			DataContext = new SettingsStatusViewModel()
		})
	};
}