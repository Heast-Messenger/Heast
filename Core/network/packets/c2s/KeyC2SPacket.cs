using Core.network.listeners;

namespace Core.network.packets.c2s; 

public class KeyC2SPacket : IPacket<IServerLoginListener> {
    
    public byte[] Key { get; }
    
    // @Müd!!!!! Hüfe!!!!
    // public KeyC2SPacket(SecretKey secretKey, PublicKey publicKey) {...}

    public KeyC2SPacket(PacketBuf buffer) {
        Key = buffer.ReadByteArray();
    }
    
    public void Write(PacketBuf buf) {
        buf.WriteByteArray(Key);
    }

    public void Apply(IServerLoginListener listener) {
        listener.OnKey(this);
    }
}