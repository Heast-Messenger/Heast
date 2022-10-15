package heast.core.network.listeners;

import heast.core.network.PacketListener;
import heast.core.network.packets.c2s.login.KeyC2SPacket;
import heast.core.network.packets.c2s.login.HelloC2SPacket;

public interface ServerLoginListener extends PacketListener {

    /**
     * Called when the client wants to connect to the server.
     */
    void onHello(HelloC2SPacket packet);

    /**
     * Called when the client sends their symmetric communication key.
     */
    void onKey(KeyC2SPacket packet);
}
