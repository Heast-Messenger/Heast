package heast.core.network.packets.c2s.login;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.listeners.ServerLoginListener;
import heast.core.security.RSA;

import javax.crypto.SecretKey;
import java.security.PublicKey;

public final class KeyC2SPacket implements Packet<ServerLoginListener> {

    /**
     * The encrypted secret key.
     */
    final byte[] key;

    public KeyC2SPacket(SecretKey secretKey, PublicKey publicKey) {
        // Manually encrypt the secret AES key, until automatic encryption is activated
        this.key = RSA.encrypt(publicKey, secretKey.getEncoded());
    }

    public KeyC2SPacket(PacketBuf buf) {
        this.key = buf.readByteArray();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeByteArray(key);
    }

    @Override
    public void apply(ServerLoginListener listener) {
        listener.onKey(this);
    }

    public byte[] getKey() {
        return key;
    }
}
