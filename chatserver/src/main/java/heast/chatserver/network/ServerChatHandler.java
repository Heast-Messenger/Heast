package heast.chatserver.network;

import heast.core.network.ClientConnection;
import heast.core.network.UserAccount;
import heast.core.network.c2s.*;
import heast.core.network.c2s.listener.ServerChatListener;
import heast.core.network.s2c.*;
import heast.core.utility.Validator;

public class ServerChatHandler implements ServerChatListener {

        private final ClientConnection connection;

    public ServerChatHandler(ClientConnection connection) {
            this.connection = connection;
        }


    @Override
    public void onTest(TestC2SPacket buf) {
        System.out.println("Client Test-Package received");
        connection.send(
                new TestS2CPacket()
        );
    }

    @Override
    public void onServerKeyRequest(ServerKeyC2SPacket buf) {
        System.out.println("Requesting server key");
        /*connection.send(
                new ServerKeyResponseS2CPacket(
                        ServerNetwork.getServerKey().getPublicKey(),
                        ServerNetwork.getServerKey().getModulus()
                )
        );*/
    }

    @Override
    public void onClientKeyRequest() {
        System.out.println("Requesting client key");
    }

    @Override
    public void onMessageReceived() {
        System.out.println("Receiving message from client");
    }
}
