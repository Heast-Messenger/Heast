using System;
using Avalonia;
using Client.Extensions;
using Client.Services;
using Client.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

internal static class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called:
    //  things aren't initialized yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
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
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<NetworkService>()
            .AddTransient<ConnectionViewModel>();

        services.AddTransient<MainWindowViewModel>();

        services.AddTransient<LoginWindowViewModel>();
    }
}