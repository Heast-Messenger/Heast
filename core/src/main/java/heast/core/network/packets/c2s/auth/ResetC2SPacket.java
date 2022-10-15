package heast.core.network.packets.c2s.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ServerAuthListener;

public final class ResetC2SPacket implements Packet<ServerAuthListener> {

    private final String email;
    private final String password;

    public ResetC2SPacket(String email, String password) {
        this.email = email;
        this.password = password;
    }

    public ResetC2SPacket(PacketBuf buf) {
        this.email = buf.readString();
        this.password = buf.readString();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeString(email);
        buf.writeString(password);
    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onReset(this);
    }

    public String getEmail() {
        return email;
    }

    public String getPassword() {
        return password;
    }
}
