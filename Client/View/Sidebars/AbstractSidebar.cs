using Avalonia;
using Avalonia.Controls;

namespace Client.View.Sidebars; 

public abstract class AbstractSidebar : UserControl {
	protected AbstractSidebar() {
		this.Padding = new Thickness(10.0);
	}
}