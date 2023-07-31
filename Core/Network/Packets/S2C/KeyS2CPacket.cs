using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class KeyS2CPacket : AbstractPacket
{
    public KeyS2CPacket()
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