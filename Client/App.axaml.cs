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

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			desktop.MainWindow = desktop.Args?[0] switch
			{
				"--home" => new MainWindow
				{
					DataContext = new MainWindowViewModel()
				},
				"--login" => new LoginWindow
				{
					DataContext = new LoginWindowViewModel()
				},
				_ => throw new ArgumentOutOfRangeException()
			};
	}
}
