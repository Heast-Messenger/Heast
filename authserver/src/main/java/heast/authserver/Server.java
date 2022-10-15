package heast.authserver;

import heast.core.logging.IO;
import heast.core.network.*;
import heast.core.network.pipeline.ClientConnection;
import heast.core.network.pipeline.PacketDecoder;
import heast.core.network.pipeline.PacketEncoder;
import io.netty.bootstrap.ServerBootstrap;
import io.netty.channel.*;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioServerSocketChannel;
import heast.authserver.components.Email;
import heast.authserver.components.Database;
import heast.authserver.network.ServerLoginHandler;
import heast.authserver.network.ServerNetwork;

public final class Server {

	public static void main(String... args){
		int port = args.length > 0
		  ? Integer.parseInt(args[0])
		  : 8080;

		start(port);
	}

	public static void start(int port) {
		var bossGroup = new NioEventLoopGroup();
		var workerGroup = new NioEventLoopGroup();
		try {
			new ServerBootstrap()
				.group(bossGroup, workerGroup)
				.channel(NioServerSocketChannel.class)
				.childHandler(new ChannelInitializer<SocketChannel>() {
					public void initChannel(SocketChannel ch) {
						ch.config().setOption(ChannelOption.TCP_NODELAY, true);
						var connection = new ClientConnection(NetworkSide.SERVER);
						connection.setListener(new ServerLoginHandler(connection));
						ch.pipeline()
							// Here will be the packet decryptor
							.addLast("decoder", new PacketDecoder(NetworkSide.CLIENT))
							// Here will be the packet encryptor
							.addLast("encoder", new PacketEncoder(NetworkSide.SERVER))
							.addLast("handler", connection);
					}
				})
				.option(ChannelOption.SO_BACKLOG, 128)
				.childOption(ChannelOption.SO_KEEPALIVE, true)
				.childOption(ChannelOption.TCP_NODELAY, true)
				.bind(port)
				.syncUninterruptibly().addListener(future -> {
					if (future.isSuccess()) {
						ServerNetwork.initialize();
						Database.initialize();
						Email.initialize();
						IO.info.println("Server started on port " + port + ".");
					}
				})
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