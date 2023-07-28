using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class VerifyEmailS2CPacket : AbstractPacket
{
    public enum ResponseStatus
    {
        WrongCode,
        Unauthorized,
        Success
    }

    public VerifyEmailS2CPacket(ResponseStatus status)
    {
        Status = status;
    }

    public VerifyEmailS2CPacket(PacketBuf buf)
    {
        Status = buf.ReadEnum<ResponseStatus>();
    }

    public ResponseStatus Status { get; }

    public override void Write(PacketBuf buf)
    {
        buf.WriteEnum(Status);
    }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientAuthListener authListener)
        {
            authListener.OnVerifyResponse(this);
        }
    }
}