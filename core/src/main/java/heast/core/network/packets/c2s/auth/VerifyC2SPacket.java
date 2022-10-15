package heast.core.network.packets.c2s.auth;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ServerAuthListener;

public final class VerifyC2SPacket implements Packet<ServerAuthListener> {

    private final String code;

    public VerifyC2SPacket(String code) {
        this.code = code;
    }

    public VerifyC2SPacket(PacketBuf buf) {
        this.code = buf.readString();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeString(code);
    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onVerify(this);
    }

    public String getCode() {
        return code;
    }
}
