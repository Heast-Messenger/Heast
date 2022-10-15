package heast.core.network.pipeline;

import heast.core.logging.IO;
import heast.core.security.AES;
import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.MessageToMessageDecoder;

import javax.crypto.SecretKey;
import java.util.List;

public final class PacketDecryptor extends MessageToMessageDecoder<ByteBuf> {

    private final SecretKey key;

    public PacketDecryptor(SecretKey key) {
        this.key = key;
    }

    @Override
    protected void decode(ChannelHandlerContext ctx, ByteBuf msg, List<Object> out) {
        var readable = msg.readableBytes();
        var bytes = new byte[readable];
        msg.readBytes(bytes);

        var buf = ctx.alloc().heapBuffer();
        var decrypted = AES.decrypt(key, bytes);
        buf.writeBytes(decrypted);
        out.add(buf);
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        IO.error.println("Error while decrypting packet: " + cause.getMessage());
        cause.printStackTrace();
    }
}
