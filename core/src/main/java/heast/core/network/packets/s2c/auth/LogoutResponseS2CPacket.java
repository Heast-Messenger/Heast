package heast.core.network.packets.s2c.auth;

import heast.core.model.UserAccount;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientAuthListener;

public final class LogoutResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        SUCCESS, ERROR
    }

    private final Status status;

    public LogoutResponseS2CPacket(Status status) {
        this.status = status;
    }

    public LogoutResponseS2CPacket(PacketBuf buf) {
        this.status = buf.readEnum(Status.class);
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeEnum(status);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onLogoutResponse(this);
    }

    public Status getStatus() {
        return status;
    }
}
