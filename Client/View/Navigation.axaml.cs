using System;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.View.Sidebars;
using Client.ViewModel;
using Client.ViewModel.Sidebars;

namespace Client.View;

public class NavigationEasing : Easing {
	public override double Ease(double t) => (1 - t) * Math.Sin(t * Math.PI * 3) / 36.0;
}

public partial class Navigation : UserControl {
	public Navigation() {
		InitializeComponent();

		PointerEnteredEvent.AddClassHandler<Button>(Button_OnHover);
	}
	
	private NavigationViewModel ViewModel => 
		(DataContext as NavigationViewModel)!;

	private static void Button_OnHover(Button sender, RoutedEventArgs args) {
		if (sender.Classes.Contains("NavButton")) {
			sender.Classes.Add("Hover");
		}
	}
	
	private void Button_OnChat(object sender, RoutedEventArgs e) {
		ViewModel.NavigateTo(new EmptySidebar());
	}
	
	private void Button_OnExplore(object sender, RoutedEventArgs e) {
		ViewModel.NavigateTo(new EmptySidebar());
	}
	
	private void Button_OnPeople(object sender, RoutedEventArgs e) {
		ViewModel.NavigateTo(new EmptySidebar());
	}
	
	private void Button_OnServers(object sender, RoutedEventArgs e) {
		ViewModel.NavigateTo(new EmptySidebar());
	}
	
	private void Button_OnSettings(object sender, RoutedEventArgs e) {
		ViewModel.NavigateTo(new SettingsSidebar
		{
			DataContext = new SettingsSidebarViewModel(ViewModel.ContentViewModel)
		});
	}
}