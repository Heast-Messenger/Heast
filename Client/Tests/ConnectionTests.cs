using System.Security.Cryptography;
using System.Threading.Tasks;
using Client.Network;
using Core.Network.Codecs;
using Core.Network.Packets.C2S;
using NUnit.Framework;

namespace Client.Tests;

[TestFixture]
internal class ConnectionTests
{
	[Test]
	public async Task ConnectToAuthServerAndHandshake()
	{
		var con = await ClientConnection.ServerConnect("127.0.0.1", 23010);
		con.Listener = new ClientHandshakeHandler(con);
		await con.Channel.WriteAndFlushAsync(new HelloC2SPacket());
		await Task.Delay(-1);
	}

	[Test]
	public async Task ConnectToAuthServerAndSendHugePacket()
	{
		var con = await ClientConnection.ServerConnect("127.0.0.1", 23010);
		await con.Channel.WriteAndFlushAsync(new KeyC2SPacket(GenBytes(), GenBytes()));
		await Task.Delay(-1);

		byte[] GenBytes()
		{
			var bytes = new byte[1024];
			RandomNumberGenerator.Fill(bytes);
			return bytes;
		}
	}
}