using Client.View.Content;

namespace Client.Model; 

public class SettingsSidebarItem : SidebarItemBase {
	public SettingsSidebarItem(string iconPath, string title, ContentBase contentBase) : base(contentBase) {
		this.IconPath = iconPath;
		this.Title = title;
	}
	
	public string IconPath { get; set; }
	public string Title { get; set; }
}