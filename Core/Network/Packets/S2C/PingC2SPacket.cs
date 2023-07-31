using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class PingS2CPacket : AbstractPacket
{
    public PingS2CPacket(long startMs)
    {
        StartMs = startMs;
    }

    public long StartMs { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientHandshakeListener handshakeListener)
        {
            handshakeListener.OnPing(this);
        }
    }
}