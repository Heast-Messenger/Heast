package heast.core.network.packets.c2s.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ServerAuthListener;

public final class DeleteC2SPacket implements Packet<ServerAuthListener> {

    private final String email;

    public DeleteC2SPacket(String email) {
        this.email = email;
    }

    public DeleteC2SPacket(PacketBuf buf) {
        this.email = buf.readString();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeString(email);
    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onDelete(this);
    }

    public String getEmail() {
        return email;
    }
}
