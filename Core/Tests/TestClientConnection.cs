using NUnit.Framework;

namespace Core.Tests;

using Network.Packets.C2S;
using Network.Pipeline;

[TestFixture]
internal class TestClientConnection {

	[Test]
	public async Task ConnectToAuthServer()
	{
		var con = await ClientConnection.ServerConnect("127.0.0.1", 23010);
		await con.Send(new HelloC2SPacket("Hello"));
	}
}