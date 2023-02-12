﻿namespace Auth;

public static class AuthServer {

    public const string Version = "1.0.0";
    public const string Build = "Dev 0.1";
    
    public static string Host { get; set; } = "localhost";
    public static int Port { get; set; } = 8080;

    public static void Main(string[] args)
    {
        try {
            Dispatcher.Dispatch(args);
        }
        catch (Exception e) {
            Dispatcher.Crash(e);
        }
    }
}