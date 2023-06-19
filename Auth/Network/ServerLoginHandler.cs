using System.Security.Cryptography;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

// using Core.Network.Packets.S2C;

namespace Auth.Network;

public class ServerLoginHandler : IServerLoginListener
{
	public ServerLoginHandler(ClientConnection ctx)
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
		System.Console.WriteLine($"Got packet: {packet.ClientInfo}");
		// TODO: Send server capabilities
		// Ctx.Send(new HelloS2CPacket(ServerNetwork.PublicKey));
	}

	/// <summary>
	///     Called when the client sends their symmetric communication key.
	/// </summary>
	/// <param name="packet">The received packet containing the client's AES key.</param>
	public void OnKey(KeyC2SPacket packet)
	{
		var key = new Span<byte>();
		var iv = new Span<byte>();
		var success = ServerNetwork.KeyPair.TryDecrypt(
			packet.Key, key, RSAEncryptionPadding.OaepSHA256, out _);
		success &= ServerNetwork.KeyPair.TryDecrypt(
			packet.Iv, iv, RSAEncryptionPadding.OaepSHA256, out _);
		if (!success)
		{
			Ctx.Send(new ErrorS2CPacket(Error.InvalidKey));
			ServerNetwork.Disconnect(Ctx);
			return;
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

	/// <summary>
	///     Called when the client has experienced some kind of error.
	/// </summary>
	/// <param name="packet">The received packet containing the error type.</param>
	public void OnError(ErrorC2SPacket packet) { }
}
