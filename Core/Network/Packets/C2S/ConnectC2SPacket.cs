using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class ConnectC2SPacket : IPacket
{
	public ConnectC2SPacket(string clientInfo)
	{
		ClientInfo = clientInfo;
	}

	public ConnectC2SPacket(PacketBuf buf)
	{
		ClientInfo = buf.ReadString();
	}

	public string ClientInfo { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteString(ClientInfo);
	}

	public void Apply(IPacketListener listener)
	{
		if (listener is IServerHandshakeListener handshakeListener)
		{
			handshakeListener.OnConnect(this);
		}
	}
}