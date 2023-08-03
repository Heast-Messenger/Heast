using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class LoginC2SPacket : AbstractPacket
{
    public LoginC2SPacket(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }
    public string Password { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnLogin(this);
        }
    }
}