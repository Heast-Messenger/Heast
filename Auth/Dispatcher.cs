using System.Diagnostics;
using System.Text;
using Auth.Modules;
using Auth.Network;
using static Crayon.Output;

namespace Auth;

public static class Dispatcher
{
	private const string Prefix = "-";

	private static readonly List<(string[], string, (string[], string)[]?)> Help = new() {
		(new []{$"{Prefix}h", $"{Prefix}{Prefix}help"}, Global.Translation.ArgsHelpHelp, null),
		(new []{$"{Prefix}v", $"{Prefix}{Prefix}version"}, Global.Translation.ArgsHelpVersion, null),
		(new []{"stop"}, Global.Translation.ArgsHelpStop, null),
		(new []{"start"}, Global.Translation.ArgsHelpStart, new[] {
			(new []{$"{Prefix}i", $"{Prefix}{Prefix}ip"}, Global.Translation.ArgsHelpStartIp),
			(new []{$"{Prefix}p", $"{Prefix}{Prefix}port"}, Global.Translation.ArgsHelpStartPort),
			(new []{$"{Prefix}dbh", $"{Prefix}{Prefix}dbhost"}, Global.Translation.ArgsHelpStartDbhost),
			(new []{$"{Prefix}dbp", $"{Prefix}{Prefix}dbport"}, Global.Translation.ArgsHelpStartDbport),
		}),
	};

	private static readonly List<(string, string)> Version = new() {
		(Global.Translation.ArgsHelpVersionVersion, Global.Version),
		(Global.Translation.ArgsHelpVersionBuild, Global.Build),
		(Global.Translation.ArgsHelpVersionWebsite, Global.Website),
		(Global.Translation.ArgsHelpVersionGithub, Global.Github),
		(Global.Translation.ArgsHelpVersionDotnet, Global.DotNetInfo),
		(Global.Translation.ArgsHelpVersionOs, Global.OsInfo)
	};

	public static void Dispatch(string[] args)
	{
		if (args.Length <= 0)
		{
			PrintHelp();
			return;
		}

		foreach (var arg in args)
		{
			switch (arg)
			{
				case $"{Prefix}i" or $"{Prefix}{Prefix}ip":
					ServerNetwork.Host = arg;
					break;
				case $"{Prefix}p" or $"{Prefix}{Prefix}port":
					ServerNetwork.Port = int.Parse(arg);
					break;
				case $"{Prefix}dbh" or $"{Prefix}{Prefix}dbhost":
					Database.Host = arg;
					break;
				case $"{Prefix}dbp" or $"{Prefix}{Prefix}dbport":
					Database.Port = int.Parse(arg);
					break;
				case $"{Prefix}h" or $"{Prefix}{Prefix}help":
					PrintHelp();
					return;
				case $"{Prefix}v" or $"{Prefix}{Prefix}version":
					PrintVersion();
					return;
			}
		}

		var action = args[0];
		switch (action)
		{
			case "start":
				Start();
				return;
			case "stop":
				Stop();
				return;
		}

		throw new ArgumentException($"{Global.Translation.ArgsUnknown}".Replace("%1", action));
	}

	public static void Crash(Exception e)
	{
		PrintCrash(e);
		Environment.Exit(1);
	}

	private static void PrintCrash(Exception e)
	{
		Console.OutputEncoding = Encoding.Default;
		Console.WriteLine(Bold($"\n  {White("$h!t,")} {Red("the server crashed!")} \n"));
		Console.WriteLine($"  {White().Text("Error:")} {Bold().Red(e.Message)}");
		Console.WriteLine($"  {White().Text("Stacktrace:")} {Bold().Red(e.StackTrace != null ? $"\n{e.StackTrace}" : Bold().White("No stacktrace available."))}");
	}

	private static void PrintVersion()
	{
		Console.OutputEncoding = Encoding.Default;
		Console.WriteLine($"\n  {Bold().Cyan("Authentication Server")} {White("by")} {Bold().White("Heast Kom")} \n");
		foreach (var (description, value) in Version)
		{
			Console.WriteLine($"  {Green().Text("\u279C")}  {Bold().White().Text($"{string.Join(", ", description)}:")}".PadRight(40) + $"{value}");
		}
	}

	private static void PrintHelp()
	{
		Console.OutputEncoding = Encoding.Default;
		Console.WriteLine($"  {Global.Translation.ArgsHelpDescription}: \"./server [...]\"\n");
		foreach (var (options, description, subcommands) in Help)
		{
			Console.WriteLine($"  {Green().Text("\u279C")}  {Bold().White().Text($"{string.Join(", ", options)}:")}".PadRight(45) + $"{description}");

			if (subcommands == null) continue;
			foreach (var (subOptions, subDescription) in subcommands)
			{
				Console.WriteLine($"       {Bold().White().Text($"{string.Join(", ", subOptions)}:")}".PadRight(40) + $"{subDescription}");
			}
		}
	}

	private static void Start()
	{
		// TODO: make async
		Console.WriteLine($"> {Global.Translation.ServerStarting}");

		Database.Initialize();
		ServerBootstrap.Initialize();
		ServerNetwork.Initialize();

		Console.WriteLine($"> {Global.Translation.ServerStarted}");

		while (true)
		{
			Console.ReadLine();
		}
	}

	private static void Stop()
	{
		var processes = Process.GetProcessesByName("Auth");

		if (processes.Length <= 0)
		{
			Console.WriteLine($"{Global.Translation.ServerNoinstances}");
			return;
		}

		if (processes.Length == 1)
		{
			processes[0].Kill();
			return;
		}

		while (true)
		{
			Console.WriteLine();
			for (var i = 0; i < processes.Length; i++)
			{
				var p = processes[i];
				Console.WriteLine($"({i + 1}) {p.ProcessName} - {p.Id}");
			}
			var input = Console.ReadLine();
			if (int.TryParse(input, out var index))
			{
				if (index == 0)
				{
					Console.WriteLine($"{Global.Translation.ServerExiting}");
					return;
				}
				if (index > 0 && index <= processes.Length)
				{
					processes[index - 1].Kill();
					return;
				}
			}
		}
	}
}
