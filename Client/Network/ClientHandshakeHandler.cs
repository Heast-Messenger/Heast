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

	public async void OnHello(HelloS2CPacket packet)
	{
		using (KeyPair = Aes.Create())
		{
			KeyPair.Mode = CipherMode.CFB;
			KeyPair.Padding = PaddingMode.PKCS7;
			KeyPair.KeySize = 256;
			KeyPair.GenerateKey();
		}

		byte[] encryptedKey;
		byte[] encryptedIv;
		try
		{
			using var rsa = new RSACryptoServiceProvider();
			rsa.ImportRSAPublicKey(packet.Key, out _);
			encryptedKey = rsa.Encrypt(KeyPair.Key, RSAEncryptionPadding.Pkcs1);
			encryptedIv = rsa.Encrypt(KeyPair.IV, RSAEncryptionPadding.Pkcs1);
		}
		catch (CryptographicException e)
		{
			Console.WriteLine($"Error encrypting key: {e.Message}");
			return;
		}

		await Ctx.Send(new KeyC2SPacket(encryptedKey, encryptedIv));
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