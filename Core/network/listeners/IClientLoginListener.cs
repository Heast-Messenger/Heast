using Core.network.packets.s2c;

namespace Core.network.listeners; 

public interface IClientLoginListener : IPacketListener {
    void OnHello(HelloS2CPacket packet);
    void OnSuccess();
}