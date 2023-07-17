using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class HelloC2SPacket : AbstractPacket
{
    public HelloC2SPacket()
    {
    }

    public HelloC2SPacket(PacketBuf buf)
    {
    }

    public override void Write(PacketBuf buf)
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