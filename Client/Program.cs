using System;
using Avalonia;
using Client.Extensions;
using Client.Network;

namespace Client;

internal static class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called:
	//  things aren't initialized yet and stuff might break.
	[STAThread]
	public static void Main(string[] args)
	{
		BuildClientConnection()
			.StartInNewThread(args);

		BuildAvaloniaApp()
			.WithVersion(0, 0, 1)
			.StartWithClassicDesktopLifetime(args);
	}

	// Avalonia configuration.
	public static AppBuilder BuildAvaloniaApp()
	{
		return AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace();
	}

	// DotNetty configuration.
	public static ConnectionBuilder BuildClientConnection()
	{
		return ConnectionBuilder.Configure();
	}
}
