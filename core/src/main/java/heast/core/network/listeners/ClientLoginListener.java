package heast.core.network.listeners;

import heast.core.network.PacketListener;
import heast.core.network.packets.s2c.login.HelloS2CPacket;

public interface ClientLoginListener extends PacketListener {

    /**
     * Called when the server acknowledges the client's connection.
     * @param packet The packet containing the server's public RSA key.
     */
    void onHello(HelloS2CPacket packet);

    /**
     * Called when the client successfully authenticates with the server.
     */
    void onSuccess();
}
