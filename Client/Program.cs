using System;
using System.Reflection;
using Avalonia;
using Client.Extensions;
using Client.Network;
using Client.Services;
using Client.ViewModel;
using Core.Network;
using Core.Network.Codecs;
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
            .WithVersion(major: 0, minor: 0, patch: 1)
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
        services.AddScoped<NetworkService>();
        services.AddScoped<ConnectionViewModel>();
        services.AddScoped<ModalViewModel>();
        services.AddScoped<ClientConnection>(_ => new ClientConnection(NetworkSide.Client));
        services.AddScoped<ClientAuthHandler>();
        services.AddScoped<ClientHandshakeHandler>();

        // Assembly-search all classes that end in 'ViewModel'
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.Name.EndsWith("ViewModel"))
            {
                services.TryAddScoped(type);
            }
        }
    }
}