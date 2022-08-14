package heast.core.network.c2s;

import heast.core.network.c2s.listener.ServerAuthListener;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.security.RSA;

import java.math.BigInteger;

/**
 * A packet sent by the client to the server to reset their password.
 */
public final class ResetC2SPacket implements Packet<ServerAuthListener> {
    private byte[] email;
    private byte[] newPassword;

    private final BigInteger publicKey;
    private final BigInteger modulus;

    public ResetC2SPacket(String email, String newPassword, BigInteger publicKey, BigInteger modulus) {
        this.email = email.getBytes();
        this.newPassword = newPassword.getBytes();

        this.publicKey = publicKey;
        this.modulus = modulus;
    }

    public ResetC2SPacket(PacketBuf buf) {
        this.email = buf.readBytes();
        this.newPassword = buf.readBytes();

        // null, because the server knows its keys
        this.publicKey=null;
        this.modulus=null;
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeBytesEncryptRSA(new String(email),publicKey,modulus);
        buf.writeBytesEncryptRSA(new String(newPassword),publicKey,modulus);
    }

    public void decrypt(BigInteger privateKey, BigInteger modulus){
        this.email= RSA.INSTANCE.decrypt(this.email,privateKey,modulus);
        this.newPassword= RSA.INSTANCE.decrypt(this.newPassword,privateKey,modulus);
    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onReset(this);
    }

    public String getEmail() {
        return new String(this.email);
    }

    public String getNewPassword() {
        return new String(this.newPassword);
    }
}
