package heast.core.network.packets.s2c.login;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ClientLoginListener;

public final class SuccessS2CPacket implements Packet<ClientLoginListener> {

    public SuccessS2CPacket() {

    }

    public SuccessS2CPacket(PacketBuf buf) {

    }

    @Override
    public void write(PacketBuf buf) {

    }

    @Override
    public void apply(ClientLoginListener listener) {
        listener.onSuccess();
    }
}
