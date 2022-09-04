package heast.core.network.s2c.listener;

import heast.core.network.PacketListener;
import heast.core.network.s2c.*;

public interface ClientChatListener extends PacketListener {
    void onTestResponse(TestS2CPacket buf);
}
