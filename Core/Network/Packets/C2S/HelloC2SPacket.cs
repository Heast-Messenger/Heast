using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class HelloC2SPacket : AbstractPacket
{
    public HelloC2SPacket()
    {
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerHandshakeListener handshakeListener)
        {
            handshakeListener.OnHello(this);
        }
    }
}