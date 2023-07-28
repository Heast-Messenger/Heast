using Auth.Configuration;
using Auth.Structure;
using Core.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Services;

public class CommandsService : ICommandsProvider
{
    public CommandsService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }
    private DispatcherService DispatcherService => ServiceProvider.GetRequiredService<DispatcherService>();
    private NetworkService NetworkService => ServiceProvider.GetRequiredService<NetworkService>();
    private InfoService InfoService => ServiceProvider.GetRequiredService<InfoService>();
    private AuthDbConfig AuthDbConfig => ServiceProvider.GetRequiredService<AuthDbConfig>();

    private dynamic Translation => InfoService.Translation;

    public Command[] List => new Command[]
    {
        new()
        {
            Description = Translation.Args.Language,
            Short = "-l",
            Long = "--language",
            Argc = 1,
            Action = argv => InfoService.Translation = Translations.Load(argv[0])
        },
        new()
        {
            Description = Translation.Args.Help.Help,
            Short = "-h",
            Long = "--help",
            Argc = 0,
            Action = _ => DispatcherService.PrintHelp()
        },
        new()
        {
            Description = Translation.Args.Help.Version,
            Short = "-v",
            Long = "--version",
            Argc = 0,
            Action = _ => DispatcherService.PrintVersion()
        },
        new()
        {
            Description = Translation.Args.Help.Start,
            Short = "start",
            Long = "start",
            Argc = 0,
            Action = _ => DispatcherService.Start(),
            SubCommands = new Command[]
            {
                new()
                {
                    Description = Translation.Args.Start.Port,
                    Short = "-p",
                    Long = "--port",
                    Default = "23010",
                    Argc = 1,
                    Action = argv => NetworkService.Port = int.Parse(argv[0])
                },
                new()
                {
                    Description = Translation.Args.Start.DBHost,
                    Short = "-dbh",
                    Long = "--dbhost",
                    Default = "localhost",
                    Argc = 1,
                    Action = argv => AuthDbConfig.Host = argv[0]
                },
                new()
                {
                    Description = Translation.Args.Start.DBPort,
                    Short = "-dbp",
                    Long = "--dbport",
                    Default = "3306",
                    Argc = 1,
                    Action = argv => AuthDbConfig.Port = argv[0]
                },
                new()
                {
                    Description = Translation.Args.Start.SSHPfx,
                    Short = "-ssh",
                    Long = "--ssh",
                    Default = "~/.ssh/auth_server.pfx",
                    Argc = 1,
                    Action = argv => NetworkService.SetCertificate(argv[0])
                }
            }
        }
    };
}