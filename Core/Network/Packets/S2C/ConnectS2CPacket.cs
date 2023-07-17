using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class ConnectS2CPacket : AbstractPacket
{
    public ConnectS2CPacket(byte[] key)
    {
        Key = key;
    }

    public ConnectS2CPacket(PacketBuf buf)
    {
        Key = buf.ReadByteArray();
    }

    public byte[] Key { get; }

    public override void Write(PacketBuf buf)
    {
        buf.WriteByteArray(Key);
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientHandshakeListener handshakeListener)
        {
            handshakeListener.OnConnect(this);
        }
    }
}