namespace Core.network.listeners; 

public interface IClientLoginListener : IPacketListener {
    void onHello(HelloS2CPacket packet);
    void onSuccess();
}