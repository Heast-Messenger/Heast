package heast.core.network.c2s;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.c2s.listener.ServerAuthListener;

/**
 * A packet sent by the client to the server to request its public key.
 */
public final class ServerKeyC2SPacket implements Packet<ServerAuthListener> {

    public ServerKeyC2SPacket() {

    }

    public ServerKeyC2SPacket(PacketBuf buf) {

    }

    @Override
    public void write(PacketBuf buf) {

    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onServerKeyRequest(this);
    }
}
