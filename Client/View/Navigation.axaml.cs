using System;
using System.Threading.Tasks;
using Avalonia.Animation.Animators;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Client.View; 

public partial class Navigation : UserControl {
	public Navigation() {
		InitializeComponent();
	}

	private void Button_OnChat(object sender, RoutedEventArgs e) {
		Console.WriteLine("clicked chat");

		Dispatcher.UIThread.InvokeAsync(async () => {
			await Task.Delay(1000);
			(sender as Button)!.IsVisible = false;
			await Task.Delay(1000);
			Console.WriteLine("hiding chat button done");
		});
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

public class NavigationEasing : Easing {
	public override double Ease(double t) => (1 - t) * Math.Sin(t * Math.PI * 3) / 36.0;
}