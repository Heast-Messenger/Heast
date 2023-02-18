using Core.Network.Pipeline;
using System.Security.Cryptography;
using Auth.Modules.Database;
using Microsoft.Extensions.DependencyInjection;

// using Auth.Modules.Database;

namespace Auth.Network; 

public static class ServerNetwork {
	
	public static string Host { get; set; } = "localhost";
	public static int Port { get; set; } = 8080;
	public static CancellationToken CancellationToken { get; } = new();
	private static List<ClientConnection> Clients { get; } = new();

	public static RSACryptoServiceProvider KeyPair { get; } = new(4096);
	public static byte[] PublicKey => KeyPair.ExportRSAPublicKey();
	public static byte[] PrivateKey => KeyPair.ExportRSAPrivateKey();
	public static Context Db => Database.Db;
	
	public static Task Initialize() {
		Console.WriteLine("Initializing server network...");
		return Task.CompletedTask;
	}
	
	public static Task Disconnect(ClientConnection connection) {
		Clients.Remove(connection);
		return Task.CompletedTask;
	}
}
