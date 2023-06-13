using Auth.Modules;
using Auth.Network;
using Auth.Structure;

namespace Auth.Console;

public static class Commands
{
	public static string Prefix => Command.Prefix;
	public static Translation Translation => Global.Translation;

	public static Command[] List { get; } =
	{
		new()
		{
			Description = Translation.ArgsHelpHelp,
			Short = $"{Prefix}h",
			Long = $"{Prefix}{Prefix}help",
			Argc = 0,
			Action = _ => Dispatcher.PrintHelp()
		},
		new()
		{
			Description = Translation.ArgsHelpVersion,
			Short = $"{Prefix}v",
			Long = $"{Prefix}{Prefix}version",
			Argc = 0,
			Action = _ => Dispatcher.PrintVersion()
		},
		new()
		{
			Description = Translation.ArgsHelpStart,
			Short = "start",
			Long = "start",
			Argc = 0,
			Action = _ => Dispatcher.Start(),
			SubCommands = new Command[]
			{
				new()
				{
					Description = Translation.ArgsHelpStartPort,
					Short = $"{Prefix}p",
					Long = $"{Prefix}{Prefix}port",
					Default = "23010",
					Argc = 1,
					Action = argv => ServerNetwork.Port = int.Parse(argv[0])
				},
				new()
				{
					Description = Translation.ArgsHelpStartDbhost,
					Short = $"{Prefix}dbh",
					Long = $"{Prefix}{Prefix}dbhost",
					Default = "localhost",
					Argc = 1,
					Action = argv => Database.Host = argv[0]
				},
				new()
				{
					Description = Translation.ArgsHelpStartDbport,
					Short = $"{Prefix}dbp",
					Long = $"{Prefix}{Prefix}dbport",
					Default = "3306",
					Argc = 1,
					Action = argv => Database.Port = int.Parse(argv[0])
				}
			}
		}
	};
}