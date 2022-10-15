package heast.core.network.listeners;

import heast.core.network.PacketListener;
import heast.core.network.packets.c2s.auth.*;

public interface ServerAuthListener extends PacketListener {

    /**
     * Called when the client registers a new account on the platform
     */
    void onSignup(SignupC2SPacket packet);

    /**
     * Called when the client logs into their account
     */
    void onLogin(LoginC2SPacket packet);

    /**
     * Called when the client resets their account password
     * Note that the password parameter is the new account password
     */
    void onReset(ResetC2SPacket packet);

    /**
     * Called when the client deletes their account
     */
    void onDelete(DeleteC2SPacket packet);

    /**
     * Called when the client logs out of their account
     */
    void onLogout();

    /**
     * Called when the client verifies their account
     */
    void onVerify(VerifyC2SPacket packet);
}
