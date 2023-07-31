using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class HelloS2CPacket : AbstractPacket
{
    public HelloS2CPacket(Capabilities capabilities)
    {
        Capabilities = capabilities;
    }

    public Capabilities Capabilities { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientHandshakeListener handshakeListener)
        {
            handshakeListener.OnHello(this);
        }
    }
}