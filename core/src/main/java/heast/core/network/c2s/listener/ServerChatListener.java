package heast.core.network.c2s.listener;

import heast.core.network.PacketListener;
import heast.core.network.c2s.*;

public interface ServerChatListener extends PacketListener {

    void onTest(TestC2SPacket buf);
    void onServerKeyRequest(ServerKeyC2SPacket buf);    //A client wants public key of the server
    void onClientKeyRequest();  //A client wants public key of all the other clients
    void onMessageReceived();   //A client has sent a message


    //Gate-Server-Functionality (do that after the server-chat works)
    /*
    void onMessageFetchRequest();   //A client wants to fetch his messages after going online (this server is its gate)
    void onSpecClientKeyRequest();  //A client wants the public key of a client that uses this server as its gate
    void onSpecMessageReceived();   //A client has sent a message to a client that uses this server as its gate
    */
}
