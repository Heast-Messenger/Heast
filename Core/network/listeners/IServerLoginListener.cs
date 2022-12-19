using Core.network.packets.c2s;

namespace Core.network.listeners; 

public interface IServerLoginListener : IPacketListener {
    void OnHello(HelloC2SPacket packet);
    void OnKey(KeyC2SPacket packet);
}