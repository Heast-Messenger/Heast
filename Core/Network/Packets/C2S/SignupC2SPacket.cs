using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class SignupC2SPacket : AbstractPacket
{
    public SignupC2SPacket(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnSignup(this);
        }
    }
}