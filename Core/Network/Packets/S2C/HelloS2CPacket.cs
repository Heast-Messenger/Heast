using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class HelloS2CPacket : IPacket<IClientLoginListener>
{

    public byte[] Key { get; }

    public HelloS2CPacket(byte[] key)
    {
        Key = key;
    }

    public HelloS2CPacket(PacketBuf buf)
    {
        Key = buf.ReadByteArray();
    }

    public void Write(PacketBuf buf)
    {
        buf.WriteByteArray(Key);
    }

    public void Apply(IClientLoginListener listener)
    {
        listener.OnHello(this);
    }
}