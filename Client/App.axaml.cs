using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client.View;
using Client.ViewModel;

namespace Client;

public class App : Application
{
	public override void Initialize()
	{
		Console.WriteLine("Initializing client gui...");
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		base.OnFrameworkInitializationCompleted();

		if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			// desktop.MainWindow = new LoginWindow
			// {
			// 	DataContext = new LoginWindowViewModel()
			// };
		
			desktop.MainWindow = new MainWindow
			{
				DataContext = new MainWindowViewModel()
			};
	}
}