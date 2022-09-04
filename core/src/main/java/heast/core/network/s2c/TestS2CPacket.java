package heast.core.network.s2c;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.s2c.listener.ClientChatListener;

public class TestS2CPacket implements Packet<ClientChatListener> {

    public TestS2CPacket() {}

    public TestS2CPacket(PacketBuf buf) {}

    @Override
    public void write(PacketBuf buf) {}

    @Override
    public void apply(ClientChatListener listener) {
        listener.onTestResponse(this);
    }
}
