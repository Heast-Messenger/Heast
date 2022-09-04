package heast.client;

import heast.client.control.network.ClientChatHandler;
import heast.core.network.*;
import heast.core.network.c2s.ServerKeyC2SPacket;
import heast.core.network.c2s.TestC2SPacket;
import io.netty.bootstrap.Bootstrap;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.ChannelInitializer;
import io.netty.channel.ChannelOption;
import io.netty.channel.EventLoopGroup;
import io.netty.channel.nio.NioEventLoopGroup;
import io.netty.channel.socket.SocketChannel;
import io.netty.channel.socket.nio.NioSocketChannel;
import heast.client.view.ClientGui;
import heast.client.control.network.ClientAuthHandler;
import heast.client.control.network.ClientNetwork;

public final class Client {
	private final String host;
	private final int port;

	public static void main(String... args) {
		String host = args.length > 0
			? args[0]
			: "localhost";

		int port = args.length > 1
			? Integer.parseInt(args[1])
			: 8080;

		Client client= new Client(host, port);
		//client.connectToChatServer("localhost",6969);	//needs to be on a different thread

		Runnable runnable = () -> { client.connectToChatServer("localhost",6969); };
		Thread thread = new Thread(runnable);
		thread.start();

		client.start();
	}

	public Client(String host, int port) {
		ClientGui.INSTANCE.initialize();
		this.host = host;
		this.port = port;
	}

	public void start() {
		EventLoopGroup workerGroup = new NioEventLoopGroup();
		try{
			new Bootstrap()
				.group(workerGroup)
				.channel(NioSocketChannel.class)
				.option(ChannelOption.SO_KEEPALIVE, true)
				.handler(new ChannelInitializer<SocketChannel>() {

					@Override
					public void channelInactive(ChannelHandlerContext ctx) {
						System.out.println("Disconnected from server");
					}

					@Override
					public void initChannel(SocketChannel ch) {
						ClientNetwork.INSTANCE.initialize();
						ClientConnection connection = new ClientConnection(NetworkSide.CLIENT, NetworkState.AUTHENTICATION);
						ClientNetwork.INSTANCE.connection = connection;
						connection.setListener(new ClientAuthHandler(connection));

						ch.pipeline().addLast(
							new PacketEncoder(NetworkSide.CLIENT),
							new PacketDecoder(NetworkSide.SERVER),
							connection
						);
					}
				})
				.connect(host, port).addListener(future -> {
					if (future.isSuccess()) {
						System.out.println("Connected to server");
					} else{
						System.err.println("Failed to connect to server: " + future.cause());
					}})
					.syncUninterruptibly().addListener(future -> {
						if (future.isSuccess()) {
							System.out.println("Requesting server public key...");
							ClientNetwork.INSTANCE.connection.send(
									new ServerKeyC2SPacket()
							);
						}
					})
				.channel()
				.closeFuture()
				.syncUninterruptibly();
		} finally {
			workerGroup.shutdownGracefully();
		}
	}

	public void connectToGate(){}//use later

	public void connectToChatServer(String host, int port){
		EventLoopGroup workerGroup = new NioEventLoopGroup();
		try{
			new Bootstrap()
					.group(workerGroup)
					.channel(NioSocketChannel.class)
					.option(ChannelOption.SO_KEEPALIVE, true)
					.handler(new ChannelInitializer<SocketChannel>() {

						@Override
						public void channelInactive(ChannelHandlerContext ctx) {
							System.out.println("Disconnected from chat-server");
						}

						@Override
						public void initChannel(SocketChannel ch) {	//TODO: Important Objects need to be instanced
							ClientConnection connection = new ClientConnection(NetworkSide.CLIENT, NetworkState.CHAT);
							ClientNetwork.INSTANCE.chatConnection = connection;
							connection.setListener(new ClientChatHandler(connection));

							ch.pipeline().addLast(
									new PacketEncoder(NetworkSide.CLIENT),
									new PacketDecoder(NetworkSide.SERVER),
									connection
							);
						}
					})
					.connect(host, port).addListener(future -> {
						if (future.isSuccess()) {
							System.out.println("Connected to chat-server");
						} else{
							System.err.println("Failed to connect to chat-server: " + future.cause());
						}})
					.syncUninterruptibly().addListener(future -> {
						if (future.isSuccess()) {
							/*System.out.println("Requesting server public key...");
							ClientNetwork.INSTANCE.connection.send(
									new ServerKeyC2SPacket()
							);*/

							//Sending Test-Packet
							ClientNetwork.INSTANCE.chatConnection.setState(NetworkState.CHAT);
							ClientNetwork.INSTANCE.chatConnection.send(
									new TestC2SPacket()
							);
						}
					})
					.channel()
					.closeFuture()
					.syncUninterruptibly();
		} finally {
			workerGroup.shutdownGracefully();
		}
	}
}
