package heast.core.network.pipeline;

import heast.core.logging.IO;
import heast.core.security.AES;
import io.netty.buffer.ByteBuf;
import io.netty.channel.ChannelHandlerContext;
import io.netty.handler.codec.MessageToByteEncoder;

import javax.crypto.SecretKey;

public final class PacketEncryptor extends MessageToByteEncoder<ByteBuf> {

    private final SecretKey key;

    public PacketEncryptor(SecretKey key) {
        this.key = key;
    }

    @Override
    protected void encode(ChannelHandlerContext ctx, ByteBuf msg, ByteBuf out) {
        var readable = msg.readableBytes();
        var bytes = new byte[readable];
        msg.readBytes(bytes);
        var encrypted = AES.encrypt(key, bytes);
        out.writeBytes(encrypted);
    }

    @Override
    public void exceptionCaught(ChannelHandlerContext ctx, Throwable cause) {
        IO.error.println("Error while encrypting packet: " + cause.getMessage());
    }
}
