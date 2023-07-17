using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class SuccessS2CPacket : AbstractPacket
{
    public SuccessS2CPacket()
    {
    }

    public SuccessS2CPacket(PacketBuf buf)
    {
    }

    public override void Write(PacketBuf buf)
    {
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientHandshakeListener handshakeListener)
        {
            handshakeListener.OnSuccess(this);
        }
    }
}