using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class ResetC2SPacket : AbstractPacket
{
    public ResetC2SPacket(string usernameOrEmail, string password)
    {
        UsernameOrEmail = usernameOrEmail;
        Password = password;
    }

    public string UsernameOrEmail { get; }
    public string Password { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnReset(this);
        }
    }
}