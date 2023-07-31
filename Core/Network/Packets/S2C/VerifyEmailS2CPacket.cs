using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class VerifyEmailS2CPacket : AbstractPacket
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

    public ResponseStatus Status { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientAuthListener authListener)
        {
            authListener.OnVerifyResponse(this);
        }
    }
}