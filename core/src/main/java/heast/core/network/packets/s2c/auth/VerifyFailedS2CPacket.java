package heast.core.network.packets.s2c.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientAuthListener;
import heast.core.network.listeners.ServerAuthListener;

public final class VerifyFailedS2CPacket implements Packet<ClientAuthListener> {

    public VerifyFailedS2CPacket() {

    }

    public VerifyFailedS2CPacket(PacketBuf buf) {

    }

    @Override
    public void write(PacketBuf buf) {

    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onVerifyFailed();
    }
}
