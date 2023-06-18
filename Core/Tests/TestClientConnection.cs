using Core.Network;
using Core.Network.Codecs;
using Core.Network.Packets.C2S;
using NUnit.Framework;

namespace Core.Tests;

[TestFixture]
internal class TestClientConnection
{
	// PROBLEM: The PacketEncoder expects an object of type
	// IPacket<IPacketListener> but the HelloC2SPacket is of type
	// IPacket<IServerLoginListener> which is not compatible with the given type
	// The encoder is not being called because of this and crashes the application

	// FIX: Un-generify IPacket (javascript-like, no type safety)
	// Actually results in cleaner code in NetworkState.cs,
	// but requires type casting in ALL IPacket.Apply() methods
	// public interface IPacket
	// {
	// 	void Write(PacketBuf buf);
	// 	void Apply(IPacketListener listener); <-- Unsafe (requires casting) =(
	// }

	// FIX: Remove the state machine altogether (in fact merge all states into one)
	// includes un-generifying IPacketListener
	// void Apply --> Redundant because packets have only one location to be applied to

	[Test]
	public async Task ConnectToAuthServer()
	{
		var con = await ClientConnection.ServerConnect("127.0.0.1", 23010);
		await con.Channel.WriteAndFlushAsync(new HelloC2SPacket("Hello World!"));
	}

	[Test]
	public void Generics()
	{
		var correct = new HelloC2SPacket("Hello World!") is IPacket<IPacketListener>;
		Assert.AreEqual(true, correct);
	}
}