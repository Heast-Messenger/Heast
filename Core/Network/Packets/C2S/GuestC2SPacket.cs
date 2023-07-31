using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class GuestC2SPacket : AbstractPacket
{
    public GuestC2SPacket(string username)
    {
        Username = username;
    }

    public string Username { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnGuest(this);
        }
    }
}