using Core.Network.Pipeline;
using System.Security.Cryptography;
using Auth.Modules;
using Auth.Structure;

namespace Auth.Network;

public static class ServerNetwork
{

    public static string Host { get; set; } = "localhost";
    public static int Port { get; set; } = 8080;
    public static CancellationToken CancellationToken { get; } = new();
    private static List<ClientConnection> Clients { get; } = new();

    public static RSACryptoServiceProvider KeyPair { get; } = new(4096);
    public static byte[] PublicKey => KeyPair.ExportRSAPublicKey();
    public static byte[] PrivateKey => KeyPair.ExportRSAPrivateKey();
    public static AuthContext Db => Database.Db;

    public static void Initialize()
    {
        Console.WriteLine("Initializing server network...");
    }

    public static Task Disconnect(ClientConnection connection)
    {
        Clients.Remove(connection);
        return Task.CompletedTask;
    }
}
