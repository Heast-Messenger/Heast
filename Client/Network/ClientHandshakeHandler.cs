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
		using (KeyPair = Aes.Create())
		{
			KeyPair.Mode = CipherMode.CFB;
			KeyPair.Padding = PaddingMode.PKCS7;
			KeyPair.KeySize = 256;
			KeyPair.GenerateKey();
		}

		var encryptedKey = new Span<byte>();
		var encryptedIv = new Span<byte>();
		using (var rsa = new RSACryptoServiceProvider())
		{
			rsa.ImportRSAPublicKey(packet.Key, out _);
			var success = rsa.TryEncrypt(KeyPair.Key, encryptedKey, RSAEncryptionPadding.Pkcs1, out _);
			success &= rsa.TryEncrypt(KeyPair.IV, encryptedIv, RSAEncryptionPadding.Pkcs1, out _);
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