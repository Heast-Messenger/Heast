package heast.authserver.network;

import heast.core.logging.IO;
import heast.core.network.NetworkState;
import heast.core.network.listeners.ServerLoginListener;
import heast.core.network.packets.c2s.login.HelloC2SPacket;
import heast.core.network.packets.c2s.login.KeyC2SPacket;
import heast.core.network.packets.s2c.login.HelloS2CPacket;
import heast.core.network.packets.s2c.login.SuccessS2CPacket;
import heast.core.network.pipeline.ClientConnection;
import heast.core.security.AES;
import heast.core.security.RSA;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

public final class ServerLoginHandler implements ServerLoginListener {

    private final ClientConnection connection;

    public ServerLoginHandler(ClientConnection connection) {
        this.connection = connection;
    }

    /**
     * Called when the client wants to connect to the server.
     * @param packet The received packet.
     */
    @Override
    public void onHello(HelloC2SPacket packet) {
        connection.send(new HelloS2CPacket(
            ServerNetwork.getKeyPair().getPublic().getEncoded()
        ), () -> IO.info.println("Sent server public key to client."));
    }

    /**
     * Called when the client sends their symmetric communication key.
     * @param packet The received packet.
     */
    @Override
    public void onKey(KeyC2SPacket packet) {
        var encryptedKey = packet.getKey();
        var decryptedKey = RSA.decrypt(ServerNetwork.getKeyPair().getPrivate(), encryptedKey);
        var key = new SecretKeySpec(decryptedKey, "AES");

        connection.enableEncryption(key);
        IO.info.println("Enabled encryption.");

        connection.send(new SuccessS2CPacket(), () -> {
            connection.setState(NetworkState.AUTH);
            connection.setListener(new ServerAuthHandler(connection));
        });
    }
}
