package heast.core.network.packets.c2s.login;

import heast.core.model.ClientInfo;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ServerLoginListener;

public final class HelloC2SPacket implements Packet<ServerLoginListener> {

    final ClientInfo clientInfo;

    public HelloC2SPacket(ClientInfo clientInfo) {
        this.clientInfo = clientInfo;
    }

    public HelloC2SPacket(PacketBuf buf) {
        this.clientInfo = new ClientInfo(buf.readString());
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeString(clientInfo.getOs());
    }

    @Override
    public void apply(ServerLoginListener listener) {
        listener.onHello(this);
    }

    public ClientInfo getClientInfo() {
        return clientInfo;
    }
}
