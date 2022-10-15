package heast.client.network;

import heast.core.logging.IO;
import heast.core.network.NetworkState;
import heast.core.network.packets.c2s.auth.LoginC2SPacket;
import heast.core.network.packets.c2s.login.KeyC2SPacket;
import heast.core.network.packets.s2c.login.HelloS2CPacket;
import heast.core.network.pipeline.ClientConnection;
import heast.core.network.listeners.ClientLoginListener;
import heast.core.security.AES;

import javax.crypto.SecretKey;
import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.X509EncodedKeySpec;

public final class ClientLoginHandler implements ClientLoginListener {

    private final ClientConnection connection;
    private SecretKey secretKey;

    public ClientLoginHandler(ClientConnection connection) {
        this.connection = connection;
    }

    /**
     * Called when the server acknowledges the client's connection.
     * @param packet The packet containing the server's public RSA key.
     */
    @Override
    public void onHello(HelloS2CPacket packet) {
        try {
            var encodedKeySpec = new X509EncodedKeySpec(packet.getKey());
            var publicKey = KeyFactory.getInstance("RSA")
                .generatePublic(encodedKeySpec);

            secretKey = AES.generateKey();
            connection.send(new KeyC2SPacket(secretKey, publicKey), () -> {
                connection.enableEncryption(secretKey);
                IO.info.println("Enabled encryption.");
            });
        } catch (NoSuchAlgorithmException | InvalidKeySpecException e) {
            IO.error.println("Failed to construct RSA key.");
        }
    }

    /**
     * Called when the client successfully authenticates with the server.
     */
    @Override
    public void onSuccess() {
        connection.setState(NetworkState.AUTH);
        connection.setListener(new ClientAuthHandler(connection));
    }
}