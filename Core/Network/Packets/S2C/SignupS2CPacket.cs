using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public partial class SignupS2CPacket : AbstractPacket
{
    public enum ResponseStatus
    {
        Error,
        AwaitingConfirmation
    }

    public SignupS2CPacket(ResponseStatus responseStatus)
    {
        Status = responseStatus;
    }

    public ResponseStatus Status { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IClientAuthListener authListener)
        {
            authListener.OnSignupResponse(this);
        }
    }
}