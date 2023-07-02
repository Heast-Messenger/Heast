// using Core.Network.Packets.S2C;

using Core.Network.Packets.S2C;

namespace Core.Network.Listeners;

public interface IClientHandshakeListener : IPacketListener
{
	void OnHello(HelloS2CPacket packet);
	void OnConnect(ConnectS2CPacket packet);
	void OnSuccess(SuccessS2CPacket packet);
	void OnError(ErrorS2CPacket packet);
}