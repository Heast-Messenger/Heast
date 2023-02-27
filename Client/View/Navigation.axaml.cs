using System;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.Model;

namespace Client.View;

public class NavigationEasing : Easing {
	public override double Ease(double t) => (1 - t) * Math.Sin(t * Math.PI * 3) / 36.0;
}

public partial class Navigation : UserControl {
	public Navigation() {
		InitializeComponent();

		PointerEnteredEvent.AddClassHandler<Button>(Button_OnHover);
	}

	private void Button_OnHover(Button sender, RoutedEventArgs args) {
		if (sender.Classes.Contains("NavButton")) {
			sender.Classes.Add("Hover");
		}
	}

	private void Button_OnChat(object sender, RoutedEventArgs e) {
		Console.WriteLine("clicked chat");
	}
	
	private void Button_OnExplore(object sender, RoutedEventArgs e) {
		Console.WriteLine("clicked explore");
	}
	
	private void Button_OnPeople(object sender, RoutedEventArgs e) {
		Console.WriteLine("clicked people");
	}
	
	private void Button_OnServers(object sender, RoutedEventArgs e) {
		Console.WriteLine("clicked servers");
	}
	
	private void Button_OnSettings(object sender, RoutedEventArgs e) {
		Console.WriteLine("clicked settings");
	}
}