using System;
using System.Net;
using System.Threading.Tasks;
using Core.Network.Packets.C2S;
using Core.Network.Pipeline;

namespace Client.Network;

public static class ClientNetwork
{
	public static ClientConnection? Ctx { get; set; }

	public static void Initialize()
	{
		Console.WriteLine("Initializing client network...");
	}

	public static async Task<string> Resolve(string domain)
	{
		Console.WriteLine($"Resolving {domain}...");
		var dns = await Dns.GetHostEntryAsync(domain);
		if (dns.AddressList.Length > 0)
		{
			var ip = dns.AddressList[0];
			return ip.ToString();
		}

		throw new("Invalid server address");
	}

	public static async void Connect(string host, int port)
	{
		Console.WriteLine($"Connecting to {host}:{port}...");
		Ctx = await ClientConnection.GetServerConnection(host, port);
		Ctx.Listener = new ClientLoginHandler(Ctx);
		Handshake();
	}

	public static async void Handshake()
	{
		if (Ctx?.Channel is {Open: true})
		{
			await Ctx.Send(new HelloC2SPacket("Heast Client"));
		}
	}
}