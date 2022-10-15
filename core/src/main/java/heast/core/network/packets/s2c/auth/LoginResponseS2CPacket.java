package heast.core.network.packets.s2c.auth;

import heast.core.model.UserAccount;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientAuthListener;

public final class LoginResponseS2CPacket implements Packet<ClientAuthListener> {

    public enum Status {
        SUCCESS, AWAIT_VERIFICATION, INVALID_CREDENTIALS, USER_NOT_FOUND, ERROR
    }

    private final Status status;
    private final UserAccount account;

    public LoginResponseS2CPacket(Status status, UserAccount account) {
        this.status = status;
        this.account = account;
    }

    public LoginResponseS2CPacket(PacketBuf buf) {
        this.status = buf.readEnum(Status.class);
        this.account = buf.readUser();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeEnum(status);
        buf.writeUser(account);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onLoginResponse(this);
    }

    public Status getStatus() {
        return status;
    }

    public UserAccount getAccount() {
        return account;
    }
}
