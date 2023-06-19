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
		WriteLine(Parser.ParseRichText(content, new()
		{
			{"ArgsHelpVersionVersion", Global.Translation.ArgsHelpVersionVersion},
			{"ArgsHelpVersionBuild", Global.Translation.ArgsHelpVersionBuild},
			{"ArgsHelpVersionWebsite", Global.Translation.ArgsHelpVersionWebsite},
			{"ArgsHelpVersionGithub", Global.Translation.ArgsHelpVersionGithub},
			{"ArgsHelpVersionDotnet", Global.Translation.ArgsHelpVersionDotnet},
			{"ArgsHelpVersionOs", Global.Translation.ArgsHelpVersionOs},

			{"Version", Global.Version},
			{"Build", Global.Build},
			{"Website", Global.Website},
			{"Github", Global.Github},
			{"Dotnet", Global.DotNetInfo},
			{"Os", Global.OsInfo}
		}));
	}

	public override void PrintHelp()
	{
		Clear();
		OutputEncoding = Encoding.Default;
		{
			var content = File.ReadAllText("Assets/Console/Help.txt");
			WriteLine(Parser.ParseRichText(content, new()
			{
				{"ArgsHelpDescription", Global.Translation.ArgsHelpDescription}
			}));
		}

		PrintHelp(CommandsProvider.List);
		WriteLine("");
	}

	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	public override void Start()
	{
		WriteLine($"> {Global.Translation.ServerStarting}");

		var stopWatch = new Stopwatch();
		stopWatch.Start();

		//		Database.Initialize();
		ServerBootstrap.Initialize();
		ServerNetwork.Initialize();

		stopWatch.Stop();
		var time = stopWatch.ElapsedMilliseconds;

		WriteLine($"> {Global.Translation.ServerStarted}");

		Clear();
		OutputEncoding = Encoding.Default;
		var content = File.ReadAllText("Assets/Console/Start.txt");
		WriteLine(Parser.ParseRichText(content, new()
		{
			{"Version", Global.Version},
			{"Time", time.ToString()},
			{"Port", ServerNetwork.Port.ToString()},
			// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
			{"Db", Database.Db == null ? "§4No database connected" : $"{Database.Db.Database.ProviderName}@{Database.Host}:{Database.Port}"}
		}));

		while (true)
		{
			var command = ReadLine();
		}
	}
}
