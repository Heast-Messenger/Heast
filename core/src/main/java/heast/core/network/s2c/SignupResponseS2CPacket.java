package heast.core.network.s2c;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.s2c.listener.ClientAuthListener;

/**
 * A packet sent by the server to the client to tell them if they have successfully signed up with an account or not.
 */
public final class SignupResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        OK, CODE_SENT, INVALID_CREDENTIALS, USER_EXISTS
    }

    private final Status status;

    public SignupResponseS2CPacket(Status status) {
        this.status = status;
    }

    public SignupResponseS2CPacket(PacketBuf buf) {
        this.status = buf.readEnum(Status.class);
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeEnum(status);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onSignupResponse(this);
    }

    public Status getStatus() {
        return status;
    }
}
