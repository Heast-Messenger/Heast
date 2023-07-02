using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Auth.Modules;
using Auth.Structure;
using Core.Network;
using Core.Network.Codecs;
using Core.Utility;
using static System.Console;

namespace Auth.Network;

public static class ServerNetwork
{
	public static int Port { get; set; } = 23010;
	public static CancellationToken CancellationToken { get; } = new();
	private static List<ClientConnection> Clients { get; } = new();
	public static Capabilities Capabilities { get; set; } = Capabilities.None;
	public static RSACryptoServiceProvider KeyPair { get; } = new(4096);
	public static X509Certificate2? Certificate { get; private set; }
	public static AuthContext? Db => Database.Db;

	public static void Initialize()
	{
		WriteLine("Initializing server network...");
		if (Certificate is not null)
		{
			Capabilities |= Capabilities.Ssl;
		}
	}

	public static Task Disconnect(ClientConnection connection)
	{
		Clients.Remove(connection);
		return Task.CompletedTask;
	}

	public static void SetCertificate(string filepath)
	{
		try
		{
			// Doesnt have private key?!??!
			Certificate = new X509Certificate2(filepath, Shared.Config["ssh-password"]);
		}
		catch (CryptographicException e)
		{
			WriteLine($"Error creating SSL certificate: {e.Message}");
		}
	}
}