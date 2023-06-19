using System.Diagnostics.CodeAnalysis;
using System.Text;
using Auth.Console;
using Auth.Modules;
using Auth.Network;
using Core.Extensions;
using Core.Server;
using static System.Console;

namespace Auth;

public class Dispatcher : AbstractDispatcher
{
	public override ICommandsProvider CommandsProvider { get; } = new Commands();

	public override void PrintVersion()
	{
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
		OutputEncoding = Encoding.Default;
		{
			var content = File.ReadAllText("Assets/Console/Help.txt");
			WriteLine(Parser.ParseRichText(content, new()
			{
				{"ArgsHelpDescription", Global.Translation.ArgsHelpDescription}
			}));
		}

		PrintHelp(CommandsProvider.List);
	}

	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	public override void Start()
	{
		WriteLine($"> {Global.Translation.ServerStarting}");

//		Database.Initialize();
		ServerBootstrap.Initialize();
		ServerNetwork.Initialize();

		WriteLine($"> {Global.Translation.ServerStarted}");

		while (true)
		{
			var command = ReadLine();
		}
	}

}
