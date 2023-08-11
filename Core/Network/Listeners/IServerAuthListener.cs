using Core.Network.Packets.C2S;

namespace Core.Network.Listeners;

public interface IServerAuthListener : IPacketListener
{
    void OnSignup(SignupC2SPacket packet);
    void OnLogin(LoginC2SPacket packet);
    void OnReset(ResetC2SPacket packet);
    void OnDelete(DeleteC2SPacket packet);
    void OnLogout(LogoutC2SPacket packet);
    void OnVerifyEmail(VerifyEmailC2SPacket packet);
    void OnGuest(GuestC2SPacket packet);
    void OnAccountRequest(AccountRequestC2SPacket packet);
}