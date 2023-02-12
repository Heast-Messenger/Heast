using System.Diagnostics;
using Auth.Network;
using Crayon;
using Microsoft.VisualBasic;
using static Crayon.Output;

namespace Auth; 

public static class Dispatcher {
    private const string Prefix = "-";
    
    private static readonly List<(string[], string, (string[], string)[]?)> Help = new() {
        (new []{$"{Prefix}h", $"{Prefix}{Prefix}help"}, "Show this help message and exit.", null),
        (new []{$"{Prefix}v", $"{Prefix}{Prefix}version"}, "Show the version and exit.", null),
        (new []{"stop"}, "Stop running server instances.", null),
        (new []{"start"}, "Start a server instance.", new[] {
            (new []{$"{Prefix}i", $"{Prefix}{Prefix}ip"}, "The IP address to bind to."),
            (new []{$"{Prefix}p", $"{Prefix}{Prefix}port"}, "The port to bind to."),
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
        var action = args[0];
        
        foreach (var arg in args) {
            switch (arg) {
                case $"{Prefix}i" or $"{Prefix}{Prefix}ip":
                    AuthServer.Host = arg;
                    break;
                case $"{Prefix}p" or $"{Prefix}{Prefix}port":
                    AuthServer.Port = int.Parse(arg);
                    break;
                case $"{Prefix}h" or $"{Prefix}{Prefix}help":
                    PrintHelp();
                    return;
                case $"{Prefix}v" or $"{Prefix}{Prefix}version":
                    PrintVersion();
                    return;
            }
        }

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
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.WriteLine(Bold($"\n  {White("$h!t,")} {Red("the server crashed!")} \n"));
        Console.WriteLine($"  {White().Text("Error:")} {Bold().Red(e.Message)}");
        Console.WriteLine($"  {White().Text("Stacktrace:")} {Bold().Red(e.StackTrace != null ? $"\n{e.StackTrace}" : Bold().White("No stacktrace available."))}");
    }

    private static void PrintVersion() {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.WriteLine($"\n  {Green("Authentication Server")} {White("by")} {Bold().White("Heast Kom")} \n");
        foreach (var (description, value) in Version) {
            Console.WriteLine($"  {Green().Text("\u279C")}  {Bold().White().Text($"{string.Join(", ", description)}:")}".PadRight(40) + $"{value}");
        }
    }

    private static void PrintHelp() {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.WriteLine("  Commands you can use: \"./server [...]\"\n");
        foreach (var (options, description, subcommands) in Help) {
            Console.WriteLine($"  {Green().Text("\u279C")}  {Bold().White().Text($"{string.Join(", ", options)}:")}".PadRight(55) + $"{description}");

            if (subcommands == null) continue;
            foreach (var (subOptions, subDescription) in subcommands) {
                Console.WriteLine($"       {Bold().White().Text($"{string.Join(", ", subOptions)}:")}".PadRight(50) + $"{subDescription}");
            }
        }
    }

    private static async void Start() {
        await ServerNetwork.Initialize();
    }

    private static void Stop() {
        Process[] processes = Process.GetProcessesByName("Auth");

        if (processes.Length <= 0)
            goto end;
        
        if (processes.Length == 1)
            goto one;

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
        
        one:
        {
            processes[0].Kill();
            return;
        }

        end:
        {
            Console.WriteLine("No running instances found");
            return;
        }
    }
}