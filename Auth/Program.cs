using Auth.Network;
using Auth.Services;
using Core.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Auth;

public static class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        var startup = new Startup();

        startup.ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var application = serviceProvider.GetService<DispatcherService>();
        if (application is not null)
        {
            application.Dispatch(args);
        }
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // services.AddDbContext<AuthDbContext>();

        services.AddSingleton<NetworkService>();
        services.AddSingleton<BootstrapService>();
        services.AddSingleton<DispatcherService>();

        services.AddTransient<ICommandsProvider, CommandsService>();
        services.AddTransient<InfoService>();
    }
}