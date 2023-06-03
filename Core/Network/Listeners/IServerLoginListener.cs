using Core.Network.Packets.C2S;

namespace Core.Network.Listeners;

public interface IServerLoginListener : IPacketListener
{
    void OnHello(HelloC2SPacket packet);
    void OnKey(KeyC2SPacket packet);
    void OnError(ErrorC2SPacket packet);
}