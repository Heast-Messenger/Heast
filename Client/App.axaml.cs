using System;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client.Services;
using Client.View;
using Client.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Client;

public class App : Application
{
    public static string? Version { get; set; }

    public override void Initialize()
    {
        Console.WriteLine("Initializing client gui...");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        var startup = new Startup();

        startup.ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        {
            var network = serviceProvider.GetRequiredService<NetworkService>();
            new Thread(() =>
            {
                network.Initialize();
            }).Start();
        }

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = desktop.Args?[0] switch
            {
                "--home" => new MainWindow
                {
                    DataContext = serviceProvider.GetService<MainWindowViewModel>()
                },
                "--login" => new LoginWindow
                {
                    DataContext = serviceProvider.GetService<LoginWindowViewModel>()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}