package heast.core.network.s2c;

import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import heast.core.network.s2c.listener.ClientAuthListener;

/**
 * A packet sent by the server to the client to tell them if they have successfully deleted their account or not.
 */
public class DeleteAcResponseS2CPacket implements Packet<ClientAuthListener> {
        public enum Status {
            OK, CODE_SENT, INVALID_CREDENTIALS, USER_NOT_FOUND
        }

        private final Status status;

        public DeleteAcResponseS2CPacket(Status status) {
            this.status = status;
        }

        public DeleteAcResponseS2CPacket(PacketBuf buf) {
            this.status = buf.readEnum(Status.class);
        }

        @Override
        public void write(PacketBuf buf) {
            buf.writeEnum(status);
        }

        @Override
        public void apply(ClientAuthListener listener) {
            listener.onDeleteAcResponse(this);
        }

        public Status getStatus() {
            return status;
        }
    }
