using Client.View.Content;

namespace Client.Model; 

public class SettingsSidebarItem : SidebarItemBase {
	public SettingsSidebarItem(string icon, string title, ContentBase contentBase) : base(contentBase) {
		this.Icon = icon;
		this.Title = title;
	}
	
	public string Icon { get; set; }
	public string Title { get; set; }
}