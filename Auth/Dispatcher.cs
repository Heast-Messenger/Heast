using System.Diagnostics;
using System.Text;
using Auth.Model;
using Auth.Modules.Database;
//using Auth.Modules.Database;
using Auth.Network;
using Microsoft.Extensions.DependencyInjection;
using static Crayon.Output;

namespace Auth; 

public static class Dispatcher {
    private const string Prefix = "-";
    
    private static readonly List<(string[], string, (string[], string)[]?)> Help = new() {
        (new []{$"{Prefix}h", $"{Prefix}{Prefix}help"}, "Show this help message and exit.", null),
        (new []{$"{Prefix}v", $"{Prefix}{Prefix}version"}, "Show the version and exit.", null),
        (new []{"stop"}, "Stop running server instances.", null),
        (new []{"start"}, "Start a server instance.", new[] {
            (new []{$"{Prefix}i", $"{Prefix}{Prefix}ip"}, "The IP address to bind to (defaults to localhost)."),
            (new []{$"{Prefix}p", $"{Prefix}{Prefix}port"}, "The port to bind to (defaults to 8080)."),
            (new []{$"{Prefix}dbh", $"{Prefix}{Prefix}dbhost"}, "The host address for the database (defaults to localhost)."),
            (new []{$"{Prefix}dbp", $"{Prefix}{Prefix}dbport"}, "The port for the database (defaults to 3306)."),
        }),
    };

    private static readonly List<(string, string)> Version = new() {
        ("Version", AuthServer.Version),
        ("Build", AuthServer.Build),
        ("Website", "https://heast.net/"),
        ("GitHub", "https://github.com/Heast-Messenger/Heast"),
        ("DotNet", Environment.Version.ToString()),
        ("OS", $"{System.Runtime.InteropServices.RuntimeInformation.OSDescription} ({System.Runtime.InteropServices.RuntimeInformation.OSArchitecture})")
    };

    public static void Dispatch(string[] args) {
        if (args.Length <= 0) {
            PrintHelp();
            return;
        }
        
        foreach (var arg in args) {
            switch (arg) {
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
        switch (action) {
            case "start":
                Start();
                return;
            case "stop":
                Stop();
                return;
        }
        
        throw new ArgumentException($"Unknown action: '{action}'");
    }
    
    public static void Crash(Exception e) {
        PrintCrash(e);
        Environment.Exit(1);
    }

    private static void PrintCrash(Exception e) {
        Console.OutputEncoding = Encoding.Default;
        Console.WriteLine(Bold($"\n  {White("$h!t,")} {Red("the server crashed!")} \n"));
        Console.WriteLine($"  {White().Text("Error:")} {Bold().Red(e.Message)}");
        Console.WriteLine($"  {White().Text("Stacktrace:")} {Bold().Red(e.StackTrace != null ? $"\n{e.StackTrace}" : Bold().White("No stacktrace available."))}");
    }

    private static void PrintVersion() {
        Console.OutputEncoding = Encoding.Default;
        Console.WriteLine($"\n  {Bold().Cyan("Authentication Server")} {White("by")} {Bold().White("Heast Kom")} \n");
        foreach (var (description, value) in Version) {
            Console.WriteLine($"  {Green().Text("\u279C")}  {Bold().White().Text($"{string.Join(", ", description)}:")}".PadRight(40) + $"{value}");
        }
    }

    private static void PrintHelp() {
        Console.OutputEncoding = Encoding.Default;
        Console.WriteLine("  Commands you can use: \"./server [...]\"\n");
        foreach (var (options, description, subcommands) in Help) {
            Console.WriteLine($"  {Green().Text("\u279C")}  {Bold().White().Text($"{string.Join(", ", options)}:")}".PadRight(45) + $"{description}");

            if (subcommands == null) continue;
            foreach (var (subOptions, subDescription) in subcommands) {
                Console.WriteLine($"       {Bold().White().Text($"{string.Join(", ", subOptions)}:")}".PadRight(40) + $"{subDescription}");
            }
        }
    }

    private static void Start() {
        // TODO: make async
        Console.WriteLine("> Starting server...");

        Database.Initialize();
        ServerBootstrap.Initialize();
        ServerNetwork.Initialize();
        
        Console.WriteLine("> Server started!");

        while (true) {
            Console.ReadLine();
        }
    }

    private static void Stop() {
        var processes = Process.GetProcessesByName("Auth");

        if (processes.Length <= 0) {
            Console.WriteLine("No running instances found");
            return;
        }

        if (processes.Length == 1) {
            processes[0].Kill();
            return;
        }

        while (true) {
            Console.WriteLine("Which process do you want to kill? (0 to exit)");
            for (var i = 0; i < processes.Length; i++) {
                var p = processes[i];
                Console.WriteLine($"({i+1}) {p.ProcessName} - {p.Id}");
            }
            var input = Console.ReadLine();
            if (int.TryParse(input, out var index)) {
                if (index == 0) {
                    Console.WriteLine("Exiting...");
                    return;
                }
                if (index > 0 && index <= processes.Length) {
                    processes[index-1].Kill();
                    return;
                }
            }
        }
    }
}
