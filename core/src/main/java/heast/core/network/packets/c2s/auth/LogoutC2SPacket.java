package heast.core.network.packets.c2s.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ServerAuthListener;

public final class LogoutC2SPacket implements Packet<ServerAuthListener> {

    public LogoutC2SPacket() {

    }

    public LogoutC2SPacket(PacketBuf buf) {

    }

    @Override
    public void write(PacketBuf buf) {

    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onLogout();
    }
}
