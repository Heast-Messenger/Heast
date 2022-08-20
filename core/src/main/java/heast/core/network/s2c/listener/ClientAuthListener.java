package heast.core.network.s2c.listener;

import heast.core.network.PacketListener;
import heast.core.network.s2c.*;

public interface ClientAuthListener extends PacketListener {
    void onLoginResponse(LoginResponseS2CPacket buf);

    void onSignupResponse(SignupResponseS2CPacket buf);

    void onResetResponse(ResetResponseS2CPacket buf);

    void onServerPublicKeyResponse(ServerKeyResponseS2CPacket buf);

    void onDeleteAcResponse(DeleteAcResponseS2CPacket buf);
}
