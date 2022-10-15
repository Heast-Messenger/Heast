package heast.core.network.packets.s2c.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientAuthListener;

public final class DeleteResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        SUCCESS, AWAIT_VERIFICATION, INVALID_EMAIL, ERROR
    }

    private final Status status;

    public DeleteResponseS2CPacket(Status status) {
        this.status = status;
    }

    public DeleteResponseS2CPacket(PacketBuf buf) {
        this.status = buf.readEnum(Status.class);
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeEnum(status);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onDeleteResponse(this);
    }

    public Status getStatus() {
        return status;
    }
}
