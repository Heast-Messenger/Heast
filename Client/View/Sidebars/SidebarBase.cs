using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Model;
using Client.ViewModel.Sidebars;

namespace Client.View.Sidebars;

public abstract class SidebarBase : UserControl {
	private const int Insets = 8;
	protected SidebarBase() {
		this.Padding = new Thickness(
			Insets + 0.00, Insets + 10.0, Insets + 10.0, Insets + 10.0);
	}

	protected void OnSidebarItemClick<T>(object? sender, RoutedEventArgs e) where T : SidebarItemBase {
		// This code is holy! DO NOT TOUCH IT!
		var vm = DataContext as SidebarViewModelBase;
		if (sender is Button { DataContext: T item }) {
			vm!.NavigateTo(item);
		}
	}
}