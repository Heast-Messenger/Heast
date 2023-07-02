using Auth.Modules;
using Auth.Network;
using Auth.Structure;
using Core.Server;

namespace Auth.Console;

public class Commands : ICommandsProvider
{
	private static dynamic Translation => Global.Translation;

	public Command[] List => new Command[]
	{
		new()
		{
			Description = Translation.Args.Language,
			Short = "-l",
			Long = "--language",
			Argc = 1,
			Action = argv => Global.Translation = Translations.Load(argv[0])
		},
		new()
		{
			Description = Translation.Args.Help.Help,
			Short = "-h",
			Long = "--help",
			Argc = 0,
			Action = _ => Program.Dispatcher.PrintHelp()
		},
		new()
		{
			Description = Translation.Args.Help.Version,
			Short = "-v",
			Long = "--version",
			Argc = 0,
			Action = _ => Program.Dispatcher.PrintVersion()
		},
		new()
		{
			Description = Translation.Args.Help.Start,
			Short = "start",
			Long = "start",
			Argc = 0,
			Action = _ => Program.Dispatcher.Start(),
			SubCommands = new Command[]
			{
				new()
				{
					Description = Translation.Args.Start.Port,
					Short = "-p",
					Long = "--port",
					Default = "23010",
					Argc = 1,
					Action = argv => ServerNetwork.Port = int.Parse(argv[0])
				},
				new()
				{
					Description = Translation.Args.Start.DBHost,
					Short = "-dbh",
					Long = "--dbhost",
					Default = "localhost",
					Argc = 1,
					Action = argv => Database.Host = argv[0]
				},
				new()
				{
					Description = Translation.Args.Start.DBPort,
					Short = "-dbp",
					Long = "--dbport",
					Default = "3306",
					Argc = 1,
					Action = argv => Database.Port = int.Parse(argv[0])
				},
				new()
				{
					Description = Translation.Args.Start.SSHPfx,
					Short = "-ssh",
					Long = "--ssh",
					Default = "~/.ssh/auth_server.pfx",
					Argc = 1,
					Action = argv => ServerNetwork.SetCertificate(argv[0])
				}
			}
		}
	};
}