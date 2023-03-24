using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Svg.Skia;
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
		new("/Assets/Settings/Account.svg", "Account", new SettingsAccountPanel{
			DataContext = new SettingsAccountViewModel()
		}),
		new("/Assets/Settings/Security.svg", "Security", new SettingsSecurityPanel {
			DataContext = new SettingsSecurityViewModel()
		}),
		new("/Assets/Settings/Notifications.svg", "Notifications", new SettingsNotificationsPanel {
			DataContext = new SettingsNotificationsViewModel()
		}),
		new("/Assets/Settings/Appearance.svg", "Appearance", new SettingsAppearancePanel {
			DataContext = new SettingsAppearanceViewModel()
		}),
		new("/Assets/Settings/Status.svg", "Status", new SettingsStatusPanel {
			DataContext = new SettingsStatusViewModel()
		})
	};
}