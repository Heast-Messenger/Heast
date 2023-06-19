using System.Diagnostics.CodeAnalysis;
using System.Text;
using Core.Extensions;
using static System.Console;

namespace Core.Server;

public abstract class AbstractDispatcher
{
	public abstract ICommandsProvider CommandsProvider { get; }
	
	public void Dispatch(string[] args)
	{
		if (args.Length <= 0)
		{
			PrintHelp();
			return;
		}

		Dispatch(args, CommandsProvider.List);
	}

	public void Dispatch(string[] args, Command[] commands)
	{
		for (var i = 0; i < args.Length;)
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
	
	public abstract void Start();

	public virtual void Crash(Exception e)
	{
		PrintCrash(e);
		Environment.Exit(1);
	}

	private void PrintCrash(Exception e)
	{
		OutputEncoding = Encoding.Default;
		var content = File.ReadAllText("Assets/Console/Crash.txt");
		WriteLine(Parser.ParseRichText(content, new()
		{
			{"error", e.Message},
			{"stacktrace", e.StackTrace!}
		}));
	}

	public abstract void PrintVersion();

	public abstract void PrintHelp();

	public virtual void PrintHelp(Command[] commands, int indent = 0)
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

}
