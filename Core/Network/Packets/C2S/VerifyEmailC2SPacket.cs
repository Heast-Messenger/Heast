using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class VerifyEmailC2SPacket : AbstractPacket
{
    public VerifyEmailC2SPacket(string code, string email)
    {
        Code = code;
        Email = email;
    }

    public VerifyEmailC2SPacket(PacketBuf buf)
    {
        Code = buf.ReadString();
        Email = buf.ReadString();
    }

    public string Code { get; }
    public string Email { get; }

    public override void Write(PacketBuf buf)
    {
        buf.WriteString(Code);
        buf.WriteString(Email);
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnVerifyEmail(this);
        }
    }
}