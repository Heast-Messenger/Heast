package heast.core.network.c2s;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.c2s.listener.ServerChatListener;

public class TestC2SPacket  implements Packet<ServerChatListener> {

    public TestC2SPacket() {}

    public TestC2SPacket(PacketBuf buf) {}

    @Override
    public void write(PacketBuf buf) {}

    @Override
    public void apply(ServerChatListener listener) {
        listener.onTest(this);
    }
}