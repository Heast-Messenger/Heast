using Core.Network.Pipeline;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Network; 

public static class ServerNetwork {
	
	private static List<ClientConnection> Clients { get; } = new();
	public static RSACryptoServiceProvider KeyPair { get; } = new(4096);
	
	public static byte[] PublicKey => KeyPair.ExportRSAPublicKey();
	public static byte[] PrivateKey => KeyPair.ExportRSAPrivateKey();
	
	public static Task Initialize() {
		Console.WriteLine("Initializing server network...");
		return Task.CompletedTask;
	}
	
	public static Task Disconnect(ClientConnection connection) {
		Clients.Remove(connection);
		// return connection.DisconnectAsync();
		return Task.CompletedTask;
	}
}