using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class SignupC2SPacket : AbstractPacket
{
    public SignupC2SPacket(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }

    public SignupC2SPacket(PacketBuf buf)
    {
        Username = buf.ReadString();
        Email = buf.ReadString();
        Password = buf.ReadString();
    }

    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public override void Write(PacketBuf buf)
    {
        buf.WriteString(Username);
        buf.WriteString(Email);
        buf.WriteString(Password);
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnSignup(this);
        }
    }
}