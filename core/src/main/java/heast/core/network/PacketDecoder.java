package heast.core.network;

import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.ReplayingDecoder;

import java.util.List;

public final class PacketDecoder extends ReplayingDecoder<Packet<?>> {

    private final NetworkSide side;

    public PacketDecoder(NetworkSide side) {
        this.side = side;
    }

    @Override
    protected void decode(ChannelHandlerContext ctx, ByteBuf in, List<Object> out) {
        PacketBuf buffer = new PacketBuf(in);
        int packetId = buffer.readVarInt();

        Packet<?> packet = ctx.channel()
            .attr(ClientConnection.PROTOCOL_KEY).get()
            .getPacketHandler(side)
            .createPacket(packetId, buffer);

        if (packet != null) {
            out.add(packet);
        } else {
            throw new IllegalStateException("Bad packet id: " + packetId);
        }
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        System.err.println("Exception whilst decoding: " + cause.getMessage());
        cause.printStackTrace();
    }
}
