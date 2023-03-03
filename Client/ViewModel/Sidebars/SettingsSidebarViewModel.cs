using System;

namespace Client.ViewModel.Sidebars; 

public class SettingsSidebarViewModel : ViewModelBase {
	public SettingsSidebarViewModel(ContentViewModel contentViewModel) {
		_contentViewModel = contentViewModel;
	}

	public SettingsSidebarViewModel() {
		throw new InvalidOperationException("This constructor is only for design-time purposes.");
	}
	
	private readonly ContentViewModel _contentViewModel;
}