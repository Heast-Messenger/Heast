using Core.Network;
using Core.Network.Pipeline;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyBootstrap = DotNetty.Transport.Bootstrapping;
using static System.Console;

namespace Auth.Network;

public static class ServerBootstrap
{
	public static IChannel Channel { get; private set; } = null!;

	public static async void Initialize()
	{
		WriteLine("Initializing netty bootstrap...");
		var bossGroup = new MultithreadEventLoopGroup(1);
		var workerGroup = new MultithreadEventLoopGroup();

		Channel = await new NettyBootstrap.ServerBootstrap()
			.Group(bossGroup, workerGroup)
			.Channel<TcpServerSocketChannel>()
			.Option(ChannelOption.SoBacklog, 128)
			.ChildHandler(new ClientHandler())
			.BindAsync(ServerNetwork.Port);

		WriteLine($"Server listening on {ServerNetwork.Port}");

		await Task.Delay(-1, ServerNetwork.CancellationToken);

		WriteLine("Shutting down server...");

		await Task.WhenAll(
			bossGroup.ShutdownGracefullyAsync(),
			workerGroup.ShutdownGracefullyAsync());
	}
}

public class ClientHandler : ChannelInitializer<ISocketChannel>
{
	protected override void InitChannel(ISocketChannel channel)
	{
		var connection = new ClientConnection(NetworkSide.Server);
		connection.Listener = new ServerLoginHandler(connection);
		channel.Pipeline
			// Here will be the packet decryptor
			.AddLast("decoder", new PacketDecoder(NetworkSide.Client))
			// Here will be the packet encryptor
			.AddLast("encoder", new PacketEncoder(NetworkSide.Server))
			.AddLast("handler", connection);
	}
}
