package heast.core.network.s2c;

import heast.core.network.UserAccount;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.s2c.listener.ClientAuthListener;

/**
 * A packet sent by the server to the client to tell them if they were successful or not.
 */
public class LoginResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        OK, CODE_SENT, INVALID_CREDENTIALS, USER_NOT_FOUND
    }

    private final Status status;
    private final UserAccount user;

    public LoginResponseS2CPacket(Status status, UserAccount user) {
        this.status = status;
        this.user = user;
    }

    public LoginResponseS2CPacket(PacketBuf buf) {
        this.status = buf.readEnum(Status.class);
        this.user = buf.readUser();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeEnum(status);
        buf.writeUser(user);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onLoginResponse(this);
    }

    public Status getStatus() {
        return status;
    }

    public UserAccount getUser() {
        return user;
    }
}
