package heast.core.network.s2c;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.s2c.listener.ClientAuthListener;

import java.math.BigInteger;

/**
 * A packet sent by the server to the client with the public key.
 */
public final class ServerKeyResponseS2CPacket implements Packet<ClientAuthListener> {

    private final BigInteger publicKey;
    private final BigInteger modulus;

    public ServerKeyResponseS2CPacket(BigInteger publicKey, BigInteger modulus) {
        this.publicKey = publicKey;
        this.modulus = modulus;
    }

    public ServerKeyResponseS2CPacket(PacketBuf buf) {
        this.publicKey = buf.readRSAKey();
        this.modulus = buf.readModulus();
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeRSAKey(publicKey);
        buf.writeModulus(modulus);
    }

    @Override
    public void apply(ClientAuthListener listener) {
        listener.onServerPublicKeyResponse(this);
    }

    public BigInteger getPublicKey() {
        return publicKey;
    }

    public BigInteger getModulus() {
        return modulus;
    }
}
