package heast.core.network.listeners;

import heast.core.network.PacketListener;
import heast.core.network.packets.s2c.auth.*;

public interface ClientAuthListener extends PacketListener {

    /**
     * Called when the server responds after a signup request.
     */
    void onSignupResponse(SignupResponseS2CPacket packet);

    /**
     * Called when the server responds after a login request.
     */
    void onLoginResponse(LoginResponseS2CPacket packet);

    /**
     * Called when the server responds after a password reset request.
     */
    void onResetResponse(ResetResponseS2CPacket packet);

    /**
     * Called when the server responds after a account deletion request.
     */
    void onDeleteResponse(DeleteResponseS2CPacket packet);

    /**
     * Called when the server responds after a logout request.
     */
    void onLogoutResponse(LogoutResponseS2CPacket packet);

    /**
     * Called when the client fails to verify themselves
     */
    void onVerifyFailed();
}
