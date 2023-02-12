using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Animation.Animators;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.Reactive;

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