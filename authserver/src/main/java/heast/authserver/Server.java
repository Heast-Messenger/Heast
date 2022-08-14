package heast.authserver;

import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.*;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import heast.authserver.network.Email;
import heast.authserver.network.Database;
import heast.authserver.network.ServerAuthHandler;
import heast.authserver.network.ServerNetwork;
import heast.core.network.ClientConnection;
import heast.core.network.NetworkSide;
import heast.core.network.PacketDecoder;
import heast.core.network.PacketEncoder;

public final class Server {

	public static void main(String... args){
		int port = args.length > 0
		  ? Integer.parseInt(args[0])
		  : 8080;

		start(port);
	}

	public static void start(int port) {
		EventLoopGroup bossGroup = new NioEventLoopGroup();
		EventLoopGroup workerGroup = new NioEventLoopGroup();
		try {
			new ServerBootstrap()
				.group(bossGroup, workerGroup)
				.channel(NioServerSocketChannel.class)
				.handler(new ChannelInitializer<>() {
					@Override
					public void initChannel(Channel ch) {
						ch.pipeline()
							.addLast(new ChannelHandlerAdapter() {
								@Override
								public void channelActive(ChannelHandlerContext ctx) {
									ServerNetwork.initialize();
									Database.initialize();
									Email.initialize();
									System.out.println("Server active!");
								}

								@Override
								public void channelInactive(ChannelHandlerContext ctx) {
									System.out.println("Server shutdown!");
								}
							});
					}
				})
				.childHandler(new ChannelInitializer<SocketChannel>() {
					@Override
					public void initChannel(SocketChannel ch) {
						ClientConnection connection = new ClientConnection(NetworkSide.SERVER);
						connection.setListener(new ServerAuthHandler(connection));
						ch.pipeline()
							.addLast(new PacketDecoder(NetworkSide.CLIENT))
							.addLast(new PacketEncoder(NetworkSide.SERVER))
							.addLast(connection);
					}
				})
				.option(ChannelOption.SO_BACKLOG, 128)
				.childOption(ChannelOption.SO_KEEPALIVE, true)
				.bind(port)
				.syncUninterruptibly()

				.channel()
				.closeFuture()
				.syncUninterruptibly();
		}
		finally {
			workerGroup.shutdownGracefully();
			bossGroup.shutdownGracefully();
		}
	}
}