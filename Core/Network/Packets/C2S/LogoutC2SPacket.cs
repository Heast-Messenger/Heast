using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class LogoutC2SPacket : AbstractPacket
{
    public enum LogoutReason
    {
        Logout,
        Quit,
        Kick,
        Ban
    }

    public LogoutC2SPacket(LogoutReason reason)
    {
        Reason = reason;
    }

    public LogoutReason Reason { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnLogout(this);
        }
    }
}