using System.Reflection;
using Auth.Configuration;
using Auth.Network;
using Auth.Services;
using Core.Network;
using Core.Network.Codecs;
using Core.Server;
using Core.Services;
using Core.Utility;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var startup = new Startup(Shared.Config);
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(collection => startup.ConfigureServices(collection));
    }
}

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AuthDbContext>();

        services.AddSingleton(Configuration.GetSection("db").Get<AuthDbConfig>()!);
        services.AddSingleton(Configuration.GetSection("mail").Get<EmailConfig>()!);

        services.AddSingleton<NetworkService>();
        services.AddSingleton<BootstrapService>();
        services.AddScoped<DispatcherService>();

        services.AddScoped<ClientConnection>(_ => new ClientConnection(NetworkSide.Server));
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ITwoFactorService, TwoFactorService>();
        services.AddScoped< /*IInfoService*/ InfoService>();

        services.AddTransient<ICommandsProvider, CommandsService>();
        services.AddTransient<ServerAuthHandler>();
        services.AddTransient<ServerHandshakeHandler>();

        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
        services.AddValidatorsFromAssembly(Assembly.Load("Core"));
    }
}