using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class ConnectC2SPacket : AbstractPacket
{
    public ConnectC2SPacket(string clientInfo)
    {
        ClientInfo = clientInfo;
    }

    public string ClientInfo { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerHandshakeListener handshakeListener)
        {
            handshakeListener.OnConnect(this);
        }
    }
}