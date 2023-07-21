using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Server;
using static System.Console;

namespace Auth.Services;

public class DispatcherService : AbstractDispatcher
{
    public DispatcherService(
        AuthDbContext databaseService,
        ICommandsProvider commandsProvider,
        NetworkService networkService,
        BootstrapService bootstrapService,
        InfoService infoService) : base(commandsProvider)
    {
        DatabaseService = databaseService;
        CommandsProvider = commandsProvider;
        NetworkService = networkService;
        BootstrapService = bootstrapService;
        InfoService = infoService;
    }

    private AuthDbContext DatabaseService { get; }
    private ICommandsProvider CommandsProvider { get; }
    private NetworkService NetworkService { get; }
    public BootstrapService BootstrapService { get; }
    private InfoService InfoService { get; }

    public override void PrintVersion()
    {
        Clear();
        OutputEncoding = Encoding.Default;
        var content = File.ReadAllText("Assets/Console/Version.txt");
        WriteLine(Parser.ParseRichText(content, new Dictionary<string, object>
        {
            { "ArgsHelpVersionVersion", InfoService.Translation.Args.Version.Version },
            { "ArgsHelpVersionBuild", InfoService.Translation.Args.Version.Build },
            { "ArgsHelpVersionWebsite", InfoService.Translation.Args.Version.Website },
            { "ArgsHelpVersionGithub", InfoService.Translation.Args.Version.Github },
            { "ArgsHelpVersionDotnet", InfoService.Translation.Args.Version.Dotnet },
            { "ArgsHelpVersionOs", InfoService.Translation.Args.Version.Os },

            { "Version", InfoService.Version },
            { "Build", InfoService.Build },
            { "Website", InfoService.Website },
            { "Github", InfoService.Github },
            { "Dotnet", InfoService.DotNetInfo },
            { "Os", InfoService.OsInfo }
        }));
    }

    public override void PrintHelp()
    {
        Clear();
        OutputEncoding = Encoding.Default;
        {
            var content = File.ReadAllText("Assets/Console/Help.txt");
            WriteLine(Parser.ParseRichText(content, new Dictionary<string, object>
            {
                { "ArgsHelpDescription", InfoService.Translation.Args.Help.Description }
            }));
        }

        PrintHelp(CommandsProvider.List);
        WriteLine("");
    }

    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    public override void Start()
    {
        WriteLine($"> {InfoService.Translation.Server.Starting}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        Initialize();

        stopWatch.Stop();
        var time = stopWatch.ElapsedMilliseconds;

        WriteLine($"> {InfoService.Translation.Server.Started}");

        Clear();
        OutputEncoding = Encoding.Default;
        var content = File.ReadAllText("Assets/Console/Start.txt");
        WriteLine(Parser.ParseRichText(content, new Dictionary<string, object>
        {
            { "Version", InfoService.Version },
            { "Time", time },
            { "Port", NetworkService.Port },
            {
                "Db", !DatabaseService.Database.CanConnect()
                    ? "§4No database connected"
                    : $"{DatabaseService.Database.ProviderName}@{AuthDbContext.Host}:{AuthDbContext.Port}"
            },
            {
                "Certificate", NetworkService.Certificate == null
                    ? "§4No certificate specified"
                    : $"Valid {NetworkService.KeyPair.KeyExchangeAlgorithm} certificate"
            }
        }));

        while (true)
        {
            // ReSharper disable once UnusedVariable
            var command = ReadLine();
        }
    }

    private void Initialize()
    {
        NetworkService.Initialize();
        BootstrapService.Initialize();
    }
}