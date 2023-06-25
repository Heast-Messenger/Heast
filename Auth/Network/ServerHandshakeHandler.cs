using System.Security.Cryptography;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Auth.Network;

public class ServerHandshakeHandler : IServerHandshakeListener
{
	public ServerHandshakeHandler(ClientConnection ctx)
	{
		Ctx = ctx;
	}

	private ClientConnection Ctx { get; }

	/// <summary>
	///     Called when the client wants to connect to the server.
	/// </summary>
	/// <param name="packet">The received packet containing client information.</param>
	public async void OnHello(HelloC2SPacket packet)
	{
		// TODO: Send server capabilities
		var publicKey = ServerNetwork.KeyPair.ExportRSAPublicKey();
		await Ctx.Send(new HelloS2CPacket(publicKey));
	}

	/// <summary>
	///     Called when the client sends their symmetric communication key.
	/// </summary>
	/// <param name="packet">The received packet containing the client's AES key.</param>
	public async void OnKey(KeyC2SPacket packet)
	{
		byte[] key;
		byte[] iv;
		try
		{
			key = ServerNetwork.KeyPair.Decrypt(packet.Key, RSAEncryptionPadding.Pkcs1);
			iv = ServerNetwork.KeyPair.Decrypt(packet.Iv, RSAEncryptionPadding.Pkcs1);
		}
		catch (CryptographicException)
		{
			await Ctx.Send(new ErrorS2CPacket(Error.InvalidKey));
			await ServerNetwork.Disconnect(Ctx);
			return;
		}

		using var keypair = Aes.Create();
		{
			keypair.Mode = CipherMode.CFB;
			keypair.Padding = PaddingMode.PKCS7;
			keypair.Key = key;
			keypair.IV = iv;
		}

		await Ctx.Send(new SuccessS2CPacket());
		Ctx.EnableEncryption(keypair);
		Ctx.State = NetworkState.Auth;
		Ctx.Listener = new ServerAuthHandler(Ctx);
	}
}
