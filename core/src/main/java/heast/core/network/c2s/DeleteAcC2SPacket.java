package heast.core.network.c2s;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.c2s.listener.ServerAuthListener;
import heast.core.security.RSA;

import java.math.BigInteger;

/**
 * A packet sent by the client to the server to delete his account
 */
public class DeleteAcC2SPacket implements Packet<ServerAuthListener> {
        private byte[] email;

        private final BigInteger publicKey;
        private final BigInteger modulus;

        public DeleteAcC2SPacket(String email, BigInteger publicKey, BigInteger modulus) {
            this.email = email.getBytes();

            this.publicKey = publicKey;
            this.modulus = modulus;
        }

        public DeleteAcC2SPacket(PacketBuf buf) {
            this.email = buf.readBytes();

            // null, because the server knows its keys
            this.publicKey=null;
            this.modulus=null;
        }

        public void decrypt(BigInteger privateKey, BigInteger modulus){
            this.email= RSA.INSTANCE.decrypt(this.email,privateKey,modulus);
        }

        @Override
        public void write(PacketBuf buf) {
            buf.writeBytesEncryptRSA(new String(email),publicKey,modulus);
        }

        @Override
        public void apply(ServerAuthListener listener) {
            listener.onDeleteAc(this);
        }

        public String getEmail() {
            return new String(this.email);
        }
}
