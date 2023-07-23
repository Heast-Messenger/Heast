using System;
using System.Reflection;
using Avalonia;
using Client.Extensions;
using Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        services.AddSingleton<NetworkService>();

        // Assembly-search all classes that end in 'ViewModel'
        var types = Assembly.GetCallingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (type.Name.EndsWith("ViewModel"))
            {
                services.TryAddScoped(type);
            }
        }
    }
}