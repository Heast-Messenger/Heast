using System.Security.Cryptography;
using Core.Network;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;
using Core.Network.Pipeline;

namespace Auth.Network; 

public class ServerAuthHandler : IServerAuthListener {
	
	private ClientConnection Connection { get; }

	public ServerAuthHandler(ClientConnection connection) {
		this.Connection = connection;
	}
}