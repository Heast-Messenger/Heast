package heast.client.control.network;

import heast.core.network.ClientConnection;
import heast.core.network.s2c.TestS2CPacket;
import heast.core.network.s2c.listener.ClientChatListener;

public final class ClientChatHandler implements ClientChatListener {

    private final ClientConnection connection;

    public ClientChatHandler(ClientConnection connection) {
        this.connection = connection;
    }

    @Override
    public void onTestResponse(TestS2CPacket buf) {
        System.out.println("Test-Response received");
    }
}
