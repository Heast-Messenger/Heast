using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class HelloC2SPacket : IPacket<IServerLoginListener>
{

    public string ClientInfo { get; }

    public HelloC2SPacket(string clientInfo)
    {
        ClientInfo = clientInfo;
    }

    public HelloC2SPacket(PacketBuf buf)
    {
        ClientInfo = buf.ReadString();
    }

    public void Write(PacketBuf buf)
    {
        buf.WriteString(ClientInfo);
    }

    public void Apply(IServerLoginListener listener)
    {
        listener.OnHello(this);
    }
}