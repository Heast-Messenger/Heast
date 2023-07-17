using Core.Network.Packets.S2C;

namespace Core.Network.Listeners;

public interface IClientAuthListener : IPacketListener
{
    void OnError(ErrorS2CPacket packet);
    // void OnSignupResponse(SignupResponseS2CPacket packet);
    // void OnLoginResponse(LoginResponseS2CPacket packet);
    // void OnResetResponse(ResetResponseS2CPacket packet);
    // void OnDeleteResponse(DeleteResponseS2CPacket packet);
    // void OnLogoutResponse(LogoutResponseS2CPacket packet);
    // void OnVerifyFailed();
}