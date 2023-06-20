using System;
using System.Security.Cryptography;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Client.Network;

public class ClientHandshakeHandler : IClientHandshakeListener
{
	public ClientHandshakeHandler(ClientConnection ctx)
	{
		Ctx = ctx;
	}

	private ClientConnection Ctx { get; }

	private Aes? KeyPair { get; set; }

	public void OnHello(HelloS2CPacket packet)
	{
		var key = new byte[128];
		var iv = new byte[128];
		KeyPair = Aes.Create();
		{
			KeyPair.Mode = CipherMode.CFB;
			KeyPair.Padding = PaddingMode.PKCS7;
			RandomNumberGenerator.Fill(key);
			RandomNumberGenerator.Fill(iv);
		}

		var encryptedKey = new Span<byte>();
		var encryptedIv = new Span<byte>();
		RSACryptoServiceProvider rsa = new(4096);
		{
			rsa.ImportRSAPublicKey(packet.Key, out _);
			var success = rsa.TryEncrypt(key, encryptedKey, RSAEncryptionPadding.Pkcs1, out _);
			success &= rsa.TryEncrypt(iv, encryptedIv, RSAEncryptionPadding.Pkcs1, out _);

			if (!success) throw new CryptographicException("Message not encrypted.");
		}

		Ctx.Send(new KeyC2SPacket(
			encryptedKey.ToArray(),
			encryptedIv.ToArray()));
	}

	public void OnSuccess(SuccessS2CPacket packet)
	{
		Ctx.EnableEncryption(KeyPair!);
		Ctx.State = NetworkState.Auth;
		Ctx.Listener = new ClientAuthHandler(Ctx);
	}

	public void OnError(ErrorS2CPacket packet)
	{
		throw new NotImplementedException();
	}
}