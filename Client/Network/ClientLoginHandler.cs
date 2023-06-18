using System;
using System.Security.Cryptography;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Client.Network;

public class ClientLoginHandler : IClientLoginListener
{
	public ClientLoginHandler(ClientConnection ctx)
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
			var success = rsa.TryEncrypt(
				key, encryptedKey, RSAEncryptionPadding.OaepSHA256, out _);
			success &= rsa.TryEncrypt(
				iv, encryptedIv, RSAEncryptionPadding.OaepSHA256, out _);
			if (!success)
			{
				const string error = "Failed to encrypt key";
				Ctx.Send(new ErrorC2SPacket(Error.InvalidKey, error));
				throw new(error);
			}
		}

		Ctx.Send(new KeyC2SPacket(
			encryptedKey.ToArray(),
			encryptedIv.ToArray()));
	}

	public void OnSuccess()
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