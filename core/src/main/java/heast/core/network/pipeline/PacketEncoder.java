package heast.core.network.pipeline;

import heast.core.network.NetworkSide;
import heast.core.network.NetworkState;
import heast.core.network.Packet;
import heast.core.network.PacketBuf;
import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.MessageToByteEncoder;

public final class PacketEncoder extends MessageToByteEncoder<Packet<?>> {

    private final NetworkSide side;

    public PacketEncoder(NetworkSide side) {
        this.side = side;
    }

    @Override
    protected void encode(ChannelHandlerContext ctx, Packet<?> msg, ByteBuf out) {
        NetworkState state = ctx.channel().attr(ClientConnection.PROTOCOL_KEY).get();
        int packetId = state.getPacketId(side, msg);

        if (packetId != -1) {
            PacketBuf buffer = new PacketBuf(out);
            buffer.writeVarInt(packetId);
            msg.write(buffer);
        } else {
            throw new IllegalStateException("Bad packet id: " + packetId);
        }
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        System.out.println("Exception whilst encoding: " + cause.getMessage());
        cause.printStackTrace();
    }
}
