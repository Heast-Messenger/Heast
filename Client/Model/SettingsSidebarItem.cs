using Avalonia.Controls;

namespace Client.Model; 

public class SettingsSidebarItem {
	public SettingsSidebarItem(Image icon, string title) {
		this.Icon = icon;
		this.Title = title;
	}

	public SettingsSidebarItem() {
		Icon = null!;
		Title = null!;
	}
	
	public Image Icon { get; set; }
	public string Title { get; set; }
}