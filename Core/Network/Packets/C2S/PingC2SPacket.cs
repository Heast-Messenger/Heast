using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class PingC2SPacket : AbstractPacket
{
    public PingC2SPacket(long startMs)
    {
        StartMs = startMs;
    }

    public long StartMs { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerHandshakeListener handshakeListener)
        {
            handshakeListener.OnPing(this);
        }
    }
}