using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class VerifyC2SPacket : AbstractPacket
{
    public VerifyC2SPacket(string code)
    {
        Code = code;
    }

    public VerifyC2SPacket(PacketBuf buf)
    {
        Code = buf.ReadString();
    }

    public string Code { get; }

    public override void Write(PacketBuf buf)
    {
        buf.WriteString(Code);
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnVerify(this);
        }
    }
}