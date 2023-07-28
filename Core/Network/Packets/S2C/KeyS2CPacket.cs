using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class KeyS2CPacket : AbstractPacket
{
    public KeyS2CPacket()
    {
    }

    public KeyS2CPacket(PacketBuf buf)
    {
    }

    public override void Write(PacketBuf buf)
    {
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientHandshakeListener handshakeListener)
        {
            handshakeListener.OnKey(this);
        }
    }
}