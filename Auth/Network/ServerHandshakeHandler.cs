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
	public void OnHello(HelloC2SPacket packet)
	{
		// TODO: Send server capabilities
		 Ctx.Send(new HelloS2CPacket(ServerNetwork.PublicKey));
	}

	/// <summary>
	///     Called when the client sends their symmetric communication key.
	/// </summary>
	/// <param name="packet">The received packet containing the client's AES key.</param>
	public void OnKey(KeyC2SPacket packet)
	{
		var key = new Span<byte>();
		var iv = new Span<byte>();
		try
		{
			ServerNetwork.KeyPair.TryDecrypt(packet.Key, key, RSAEncryptionPadding.Pkcs1, out _);
			ServerNetwork.KeyPair.TryDecrypt(packet.Iv, iv, RSAEncryptionPadding.Pkcs1, out _);
		}
		catch (CryptographicException _)
		{
			Ctx.Send(new ErrorS2CPacket(Error.InvalidKey));
			ServerNetwork.Disconnect(Ctx);
		}

		var keypair = Aes.Create();
		{
			keypair.Mode = CipherMode.CFB;
			keypair.Padding = PaddingMode.PKCS7;
			keypair.Key = key.ToArray();
			keypair.IV = iv.ToArray();
		}

		Ctx.EnableEncryption(keypair);
		Ctx.Send(new SuccessS2CPacket());
		Ctx.State = NetworkState.Auth;
		Ctx.Listener = new ServerAuthHandler(Ctx);
	}
}
