package heast.core.network.c2s;

import heast.core.network.c2s.listener.ServerAuthListener;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.security.RSA;

import java.math.BigInteger;

/**
 * A packet sent by the client to the server to signup on the platform.
 */
public final class SignupC2SPacket implements Packet<ServerAuthListener> {
    private byte[] username;
    private byte[] email;
    private byte[] password;

    private final BigInteger publicKey;
    private final BigInteger modulus;

    public SignupC2SPacket(String username, String email, String password, BigInteger publicKey, BigInteger modulus) {
        this.username = username.getBytes();
        this.email = email.getBytes();
        this.password = password.getBytes();

        this.publicKey = publicKey;
        this.modulus = modulus;
    }

    public SignupC2SPacket(PacketBuf buf) {
        this.username = buf.readBytes();
        this.email = buf.readBytes();
        this.password = buf.readBytes();

        // null, because the server knows its keys
        this.publicKey=null;
        this.modulus=null;
    }

    public void decrypt(BigInteger privateKey, BigInteger modulus){
        this.email= RSA.INSTANCE.decrypt(this.email,privateKey,modulus);
        this.password= RSA.INSTANCE.decrypt(this.password,privateKey,modulus);
        this.username= RSA.INSTANCE.decrypt(this.username,privateKey,modulus);
    }

    @Override
    public void write(PacketBuf buf) {
        buf.writeBytesEncryptRSA(new String(username),publicKey,modulus);
        buf.writeBytesEncryptRSA(new String(email),publicKey,modulus);
        buf.writeBytesEncryptRSA(new String(password),publicKey,modulus);
    }

    @Override
    public void apply(ServerAuthListener listener) {
        listener.onSignup(this);
    }

    public String getUsername() {
        return new String(this.username);
    }

    public String getEmail() {
        return new String(this.email);
    }

    public String getPassword() {
        return new String(this.password);
    }
}
