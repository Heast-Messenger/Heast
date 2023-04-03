using System;
using Client.View.Content;

namespace Client.ViewModel.Sidebars; 

public class SettingsSidebarViewModel : SidebarViewModelBase {
	public SettingsSidebarViewModel(ContentViewModel contentViewModel) : base(contentViewModel) {
		NavigateTo(new SettingsAccountPanel());
	}

	public SettingsSidebarViewModel() {
		throw new InvalidOperationException("This constructor is only for design-time purposes.");
	}
}