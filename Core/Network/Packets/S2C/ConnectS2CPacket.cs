using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class ConnectS2CPacket : AbstractPacket
{
    public ConnectS2CPacket(byte[] key)
    {
        Key = key;
    }

    public byte[] Key { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientHandshakeListener handshakeListener)
        {
            handshakeListener.OnConnect(this);
        }
    }
}