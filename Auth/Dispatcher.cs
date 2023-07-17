using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Auth.Console;
using Auth.Modules;
using Auth.Network;
using Core.Server;
using static System.Console;

namespace Auth;

public class Dispatcher : AbstractDispatcher
{
    protected override ICommandsProvider CommandsProvider { get; } = new Commands();

    public override void PrintVersion()
    {
        Clear();
        OutputEncoding = Encoding.Default;
        var content = File.ReadAllText("Assets/Console/Version.txt");
        WriteLine(Parser.ParseRichText(content, new Dictionary<string, object>
        {
            { "ArgsHelpVersionVersion", Global.Translation.Args.Version.Version },
            { "ArgsHelpVersionBuild", Global.Translation.Args.Version.Build },
            { "ArgsHelpVersionWebsite", Global.Translation.Args.Version.Website },
            { "ArgsHelpVersionGithub", Global.Translation.Args.Version.Github },
            { "ArgsHelpVersionDotnet", Global.Translation.Args.Version.Dotnet },
            { "ArgsHelpVersionOs", Global.Translation.Args.Version.Os },

            { "Version", Global.Version },
            { "Build", Global.Build },
            { "Website", Global.Website },
            { "Github", Global.Github },
            { "Dotnet", Global.DotNetInfo },
            { "Os", Global.OsInfo }
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
                { "ArgsHelpDescription", Global.Translation.Args.Help.Description }
            }));
        }

        PrintHelp(CommandsProvider.List);
        WriteLine("");
    }

    [SuppressMessage("ReSharper", "FunctionNeverReturns")]
    public override void Start()
    {
        WriteLine($"> {Global.Translation.Server.Starting}");

        var stopWatch = new Stopwatch();
        stopWatch.Start();

        //		Database.Initialize();
        ServerBootstrap.Initialize();
        ServerNetwork.Initialize();

        stopWatch.Stop();
        var time = stopWatch.ElapsedMilliseconds;

        WriteLine($"> {Global.Translation.Server.Started}");

        Clear();
        OutputEncoding = Encoding.Default;
        var content = File.ReadAllText("Assets/Console/Start.txt");
        WriteLine(Parser.ParseRichText(content, new Dictionary<string, object>
        {
            { "Version", Global.Version },
            { "Time", time },
            { "Port", ServerNetwork.Port },
            {
                "Db",
                Database.Db == null
                    ? "§4No database connected"
                    : $"{Database.Db.Database.ProviderName}@{Database.Host}:{Database.Port}"
            },
            {
                "Certificate",
                ServerNetwork.Certificate == null
                    ? "§4No certificate specified"
                    : $"Valid {ServerNetwork.KeyPair.KeyExchangeAlgorithm} certificate"
            }
        }));

        while (true)
        {
            var command = ReadLine();
        }
    }
}