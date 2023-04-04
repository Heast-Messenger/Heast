using System;
using Client.View.Content;
using Client.ViewModel.Content;

namespace Client.ViewModel.Sidebars; 

public class SettingsSidebarViewModel : SidebarViewModelBase {
	public SettingsSidebarViewModel(ContentViewModel contentViewModel) : base(contentViewModel) {
		ContentViewModel.Content = new SettingsAccountPanel
		{
			DataContext = new SettingsAccountViewModel()
		};
	}

	public SettingsSidebarViewModel() {
		throw new InvalidOperationException("This constructor is only for design-time purposes.");
	}
}