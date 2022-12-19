namespace Core.network.listeners; 

public interface IClientAuthListener : IPacketListener {
    void OnSignupResponse(SignupResponseS2CPacket packet);
    void OnLoginResponse(LoginResponseS2CPacket packet);
    void OnResetResponse(ResetResponseS2CPacket packet);
    void OnDeleteResponse(DeleteResponseS2CPacket packet);
    void OnLogoutResponse(LogoutResponseS2CPacket packet);
    void OnVerifyFailed();
}