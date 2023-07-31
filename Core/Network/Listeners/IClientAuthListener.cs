using Core.Network.Packets.S2C;

namespace Core.Network.Listeners;

public interface IClientAuthListener : IPacketListener
{
    void OnSignupResponse(SignupS2CPacket packet);
    void OnVerifyResponse(VerifyEmailS2CPacket packet);
    void OnLoginResponse(LoginS2CPacket packet);
}