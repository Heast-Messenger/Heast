package heast.core.network.s2c;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.s2c.listener.ClientAuthListener;

/**
 * A packet sent by the server to the client to tell them if they have successfully reset their password or not.
 */
public final class ResetResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        OK, CODE_SENT, INVALID_CREDENTIALS, USER_NOT_FOUND, FAILED
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
