using System.Security.Cryptography;
using Core.Network;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;
using Core.Network.Pipeline;

namespace Auth.Network; 

public class ServerLoginHandler : IServerLoginListener {
	
	private ClientConnection Connection { get; }

	public ServerLoginHandler(ClientConnection connection) {
		this.Connection = connection;
	}
	
	/// <summary>
	/// Called when the client wants to connect to the server.
	/// </summary>
	/// <param name="packet">The received packet containing client information.</param>
	public void OnHello(HelloC2SPacket packet) {
		Connection.Send(new HelloS2CPacket(ServerNetwork.PublicKey));
	}

	/// <summary>
	/// Called when the client sends their symmetric communication key.
	/// </summary>
	/// <param name="packet">The received packet containing the client's AES key.</param>
	public void OnKey(KeyC2SPacket packet) {
		Span<byte> key = stackalloc byte[256];
		var success = ServerNetwork.KeyPair.TryDecrypt(packet.Key, key, RSAEncryptionPadding.OaepSHA256, out _);
		if (success) {
			// Connection.EnableEncryption(key);
			Connection.Send(new SuccessS2CPacket());
			Connection.SetState(NetworkState.Auth);
			Connection.SetListener(new ServerAuthHandler(Connection));
			return;
		}
		
		Connection.Send(new ErrorS2CPacket(Error.InvalidKey));
		ServerNetwork.Disconnect(Connection);
	}

	/// <summary>
	/// Called when the client has experienced some kind of error.
	/// </summary>
	/// <param name="packet">The received packet containing the error type.</param>
	public void OnError(ErrorC2SPacket packet) {
		
	}
}