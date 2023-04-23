using System;
using Core.Network.Pipeline;

namespace Client.Network;

public static class ClientNetwork
{
    public static ClientConnection Ctx { get; set; }

    public static void Initialize()
    {
        Console.WriteLine("Initializing client network...");
    }
}