using Core.Network.Packets.S2C;

namespace Core.Network.Listeners; 

public interface IClientLoginListener : IPacketListener {
    void OnHello(HelloS2CPacket packet);
    void OnSuccess();
    void OnError(ErrorS2CPacket packet);
}