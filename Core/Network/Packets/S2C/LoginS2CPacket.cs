using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class LoginS2CPacket : AbstractPacket
{
    public enum ResponseStatus
    {
        Error,
        AwaitingConfirmation
    }

    public LoginS2CPacket(ResponseStatus responseStatus)
    {
        Status = responseStatus;
    }

    public LoginS2CPacket(PacketBuf buf)
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
            authListener.OnLoginResponse(this);
        }
    }
}