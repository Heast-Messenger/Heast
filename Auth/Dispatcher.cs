using System.Diagnostics.CodeAnalysis;
using System.Text;
using Auth.Console;
using Auth.Modules;
using Auth.Network;
using Core.Extensions;
using static System.Console;

namespace Auth;

public static class Dispatcher
{
	public static void Dispatch(string[] args)
	{
		if (args.Length <= 0)
		{
			PrintHelp();
			return;
		}

		Dispatch(args, Commands.List);
	}

	public static void Dispatch(string[] args, Command[] commands)
	{
		// var i = 0;
		// foreach (var command in commands)
		// {
		// 	if (args[i] == command.Short || args[i] == command.Long)
		// 	{
		// 		i++;
		// 		if (command.SubCommands != null && args.Length > 1)
		// 		{
		// 			Dispatch(args[i..], command.SubCommands!);
		// 		}
		//
		// 		var argv = args[i..(i + command.Argc)];
		// 		command.Action(argv);
		// 		i += command.Argc;
		// 	}
		// }

		for (var i = 0; i < args.Length || i > 100000;)
		{
			foreach (var command in commands)
			{
				if (args[i] == command.Short || args[i] == command.Long)
				{
					i++;
					if (command.SubCommands != null && args.Length > 1)
					{
						Dispatch(args[i..], command.SubCommands!);
					}

					var argv = args[i..(i + command.Argc)];
					command.Action(argv);
					i += command.Argc;
					break;
				}
			}
		}
	}

	public static void Crash(Exception e)
	{
		PrintCrash(e);
		Environment.Exit(1);
	}

	private static void PrintCrash(Exception e)
	{
		OutputEncoding = Encoding.Default;
		var content = File.ReadAllText("Assets/Console/Crash.txt");
		WriteLine(Parser.ParseRichText(content, new()
		{
			{"error", e.Message},
			{"stacktrace", e.StackTrace!}
		}));
	}

	public static void PrintVersion()
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

	public static void PrintHelp()
	{
		OutputEncoding = Encoding.Default;
		{
			var content = File.ReadAllText("Assets/Console/Help.txt");
			WriteLine(Parser.ParseRichText(content, new()
			{
				{"ArgsHelpDescription", Global.Translation.ArgsHelpDescription}
			}));
		}

		PrintHelp(Commands.List);
	}

	private static void PrintHelp(Command[] commands, int indent = 0)
	{
		foreach (var command in commands)
		{
			var content = File.ReadAllText("Assets/Console/Command.txt");
			WriteLine(Parser.ParseRichText(content, new()
			{
				{"Indent", " ".Repeat(indent * 2)},
				{"Name", $"{command.Short}, {command.Long}".PadRight(20)},
				{"Description", command.Description}
			}));

			if (command.SubCommands != null)
			{
				PrintHelp(command.SubCommands, ++indent);
			}
		}
	}

	[SuppressMessage("ReSharper", "FunctionNeverReturns")]
	public static void Start()
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
