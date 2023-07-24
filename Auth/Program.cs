using System.Reflection;
using Auth.Network;
using Auth.Services;
using Core.Network;
using Core.Network.Codecs;
using Core.Server;
using Core.Utility;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth;

internal static class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        var startup = new Startup(Shared.Config);

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
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AuthDbContext>();

        services.AddSingleton<NetworkService>();
        services.AddSingleton<BootstrapService>();
        services.AddSingleton<DispatcherService>();

        services.AddScoped<ClientConnection>(_ => new ClientConnection(NetworkSide.Server));

        services.AddTransient<ICommandsProvider, CommandsService>();
        services.AddTransient<InfoService>();
        services.AddTransient<ServerAuthHandler>();
        services.AddTransient<ServerHandshakeHandler>();

        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
        services.AddValidatorsFromAssembly(Assembly.Load("Core"));
    }
}