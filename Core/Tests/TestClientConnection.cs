using Core.Network;
using Core.Network.Codecs;
using Core.Network.Packets.C2S;
using NUnit.Framework;

namespace Core.Tests;

[TestFixture]
internal class TestClientConnection
{
	[Test]
	public async Task ConnectToAuthServer()
	{
		var con = await ClientConnection.ServerConnect("127.0.0.1", 8080);
		await con.Channel.WriteAndFlushAsync(new HelloC2SPacket("Hello World!"));
		await Task.Delay(-1);
	}

	[Test]
	public void Generics()
	{
		// ReSharper disable once ConvertTypeCheckToNullCheck
		var correct = new HelloC2SPacket("Hello World!") is IPacket;
		Assert.AreEqual(true, correct);
	}
}