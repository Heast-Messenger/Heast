using Core.Network.Packets.C2S;

namespace Core.Network.Listeners;

public interface IServerHandshakeListener : IPacketListener
{
	void OnHello(HelloC2SPacket packet);
	void OnConnect(ConnectC2SPacket packet);
	void OnKey(KeyC2SPacket packet);
}