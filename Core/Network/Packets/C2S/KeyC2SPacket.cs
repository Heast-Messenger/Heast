using System.Security.Cryptography;
using Core.Network.Listeners;

namespace Core.Network.Packets.C2S; 

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