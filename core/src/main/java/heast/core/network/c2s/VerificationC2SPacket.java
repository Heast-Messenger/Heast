package heast.core.network.c2s;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.c2s.listener.ServerAuthListener;
import heast.core.security.RSA;

import java.math.BigInteger;

/**
 * A packet sent by the client to the server to verify their email address.
 */
public final class VerificationC2SPacket implements Packet<ServerAuthListener> {
    private byte[] verificationCode;

    private final BigInteger publicKey;
    private final BigInteger modulus;

    public VerificationC2SPacket(String verificationCode, BigInteger publicKey, BigInteger modulus) {
        this.verificationCode = verificationCode.getBytes();

        this.publicKey = publicKey;
        this.modulus = modulus;
    }

    public VerificationC2SPacket(PacketBuf buf) {
        this.verificationCode = buf.readBytes();

        // null, because the server knows its keys
        this.publicKey=null;
        this.modulus=null;
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeBytesEncryptRSA(new String(verificationCode),publicKey,modulus);
    }

    public void decrypt(BigInteger privateKey, BigInteger modulus){
        this.verificationCode= RSA.INSTANCE.decrypt(this.verificationCode,privateKey,modulus);
    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onVerification(this);
    }

    public String getVerificationCode() {
        return new String(verificationCode);
    }
}
