using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class AccountRequestC2SPacket : AbstractPacket
{
    public AccountRequestC2SPacket()
    {
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener serverAuthListener)
        {
            serverAuthListener.OnAccountRequest(this);
        }
    }
}