package heast.core.network.packets.s2c.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientAuthListener;

public final class ResetResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        SUCCESS, AWAIT_VERIFICATION, INVALID_CREDENTIALS, USER_NOT_FOUND, ERROR
    }

    private final Status status;

    public ResetResponseS2CPacket(Status status) {
        this.status = status;
    }

    public ResetResponseS2CPacket(PacketBuf buf) {
        this.status = buf.readEnum(Status.class);
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeEnum(status);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onResetResponse(this);
    }

    public Status getStatus() {
        return status;
    }
}
