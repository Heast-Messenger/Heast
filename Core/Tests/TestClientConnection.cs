using System.Security.Cryptography;
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
		var con = await ClientConnection.ServerConnect("127.0.0.1", 23010);
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

	[Test]
	public async Task HugePacket()
	{
		var con = await ClientConnection.ServerConnect("127.0.0.1", 23010);
		await con.Channel.WriteAndFlushAsync(new KeyC2SPacket(GenBytes(), GenBytes()));
		await Task.Delay(-1);

		byte[] GenBytes()
		{
			var bytes = new byte[512];
			RandomNumberGenerator.Fill(bytes);
			return bytes;
		}
	}

	[Test]
	public void CryptoStuff()
	{
		var example = new byte[] {0x01, 0x02, 0x03, 0x04};

		using var rsaServer = new RSACryptoServiceProvider();
		var publicKey = rsaServer.ExportRSAPublicKey();

		using var rsaClient = new RSACryptoServiceProvider();
		rsaClient.ImportRSAPublicKey(publicKey, out _);
		{
			var encrypted = rsaClient.Encrypt(example, RSAEncryptionPadding.Pkcs1);
			Console.WriteLine($"Encrypted: {string.Join(" ", encrypted)}");
		}
	}
}