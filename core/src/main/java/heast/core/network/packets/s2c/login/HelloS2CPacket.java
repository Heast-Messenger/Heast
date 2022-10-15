package heast.core.network.packets.s2c.login;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientLoginListener;

public final class HelloS2CPacket implements Packet<ClientLoginListener> {

    final byte[] key;

    public HelloS2CPacket(byte[] key) {
        this.key = key;
    }

    public HelloS2CPacket(PacketBuf buf) {
        this.key = buf.readByteArray();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeByteArray(key);
    }

    @Override
    public void apply(ClientLoginListener listener) {
        listener.onHello(this);
    }

    public byte[] getKey() {
        return key;
    }
}
