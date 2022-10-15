package heast.core.network.pipeline;

import heast.core.logging.IO;
import heast.core.network.NetworkSide;
import heast.core.network.NetworkState;
import heast.core.network.Packet;
import heast.core.network.PacketListener;
import io.netty.bootstrap.Bootstrap;
import io.netty.channel.*;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioSocketChannel;
import io.netty.util.AttributeKey;

import javax.crypto.SecretKey;

public final class ClientConnection extends SimpleChannelInboundHandler<Packet<?>> {

    public static final AttributeKey<NetworkState> PROTOCOL_KEY = AttributeKey.valueOf("protocol");

    private final NetworkSide side;
    private String verificationCode;
    private Runnable onVerifySuccess;
    private Channel channel;
    private PacketListener listener;

    public ClientConnection(NetworkSide side) {
        this.side = side;
    }

    public NetworkSide getSide() {
        return side;
    }

    public void setVerificationCode(String verificationCode) {
        this.verificationCode = verificationCode;
    }

    public String getVerificationCode() {
        return verificationCode;
    }

    public void setOnVerifySuccess(Runnable onVerifySuccess) {
        this.onVerifySuccess = onVerifySuccess;
    }

    public void onVerifySuccess() {
        onVerifySuccess.run();
    }

    public Channel getChannel() {
        return channel;
    }

    public void setState(NetworkState state) {
        channel.attr(PROTOCOL_KEY).set(state);
    }

    public NetworkState getState() {
        return channel.attr(PROTOCOL_KEY).get();
    }

    public void setListener(PacketListener listener) {
        this.listener = listener;
    }

    public PacketListener getListener() {
        return listener;
    }

    @Override
    public void channelActive(ChannelHandlerContext ctx) {
        channel = ctx.channel();
        setState(NetworkState.LOGIN);
    }

    @Override
    public void channelInactive(ChannelHandlerContext ctx) {
        if (channel.isOpen()) {
            channel.close().awaitUninterruptibly();
        }
    }

    @Override
    protected void messageReceived(ChannelHandlerContext ctx, Packet<?> msg) {
        handle(msg, listener);
    }

    @SuppressWarnings("unchecked")
    private static <T extends PacketListener> void handle(Packet<T> packet, PacketListener listener) {
        try {
            packet.apply((T) listener);
        } catch (Exception e) {
            System.err.println("ClientConnection: Error whilst handling packet, make sure to use the correct packet listener class");
            e.printStackTrace();
        }
    }

    public void send(Packet<?> packet, Runnable onSent) {
        sendInternal(packet, onSent);
    }

    public void send(Packet<?> packet) {
        sendInternal(packet, () -> {});
    }

    private void sendInternal(Packet<?> packet, Runnable onSent) {
        if (channel != null) {
            channel.writeAndFlush(packet)
                .addListener( future -> {
                    if (!future.isSuccess()) {
                        System.err.println("ClientConnection: Failed to send packet '" + packet.getClass().getSimpleName() + "'");
                        future.cause().printStackTrace();
                    } else {
                        onSent.run();
                    }
                });
        } else {
            throw new NullPointerException(
                "ClientConnection: Failed to send packet, the channel is not available"
            );
        }
    }

    public void enableEncryption(SecretKey key) {
        channel.pipeline().addBefore("encoder", "encryptor", new PacketEncryptor(key));
        channel.pipeline().addBefore("decoder", "decryptor", new PacketDecryptor(key));
    }

    public static ClientConnection connect(String host, int port) {
        var connection = new ClientConnection(NetworkSide.CLIENT);
        var workerGroup = new NioEventLoopGroup();
        new Bootstrap()
            .group(workerGroup)
            .handler(new ChannelInitializer<SocketChannel>() {
                public void initChannel(SocketChannel ch) {
                    ch.config().setOption(ChannelOption.TCP_NODELAY, true);
                    ch.pipeline()
                        // Here will be the packet decryptor
                        .addLast("decoder", new PacketDecoder(NetworkSide.SERVER))
                        // Here will be the packet encryptor
                        .addLast("encoder", new PacketEncoder(NetworkSide.CLIENT))
                        .addLast("handler", connection);
                }
            })
            .option(ChannelOption.TCP_NODELAY, true)
            .option(ChannelOption.SO_KEEPALIVE, true)
            .channel(NioSocketChannel.class)
            .connect(host, port).addListener(future -> {
                if (future.isSuccess()) {
                    IO.info.println("Connected to server");
                } else{
                    IO.error.println("Failed to connect to server: " + future.cause());
                }})
            .syncUninterruptibly();
        return connection;
    }
}
