using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public partial class VerifyEmailC2SPacket : AbstractPacket
{
    public enum VerificationPurpose
    {
        Signup,
        Login,
        Reset,
        RequestData
    }

    public VerifyEmailC2SPacket(string code, string email, VerificationPurpose verificationPurpose)
    {
        Code = code;
        Email = email;
        Purpose = verificationPurpose;
    }

    public string Code { get; }
    public string Email { get; }
    public VerificationPurpose Purpose { get; }

    public override void Apply(IPacketListener listener)
    {
        if (listener is IServerAuthListener authListener)
        {
            authListener.OnVerifyEmail(this);
        }
    }
}