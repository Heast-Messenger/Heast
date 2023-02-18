using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using NettyBootstrap = DotNetty.Transport.Bootstrapping;
using Core.Network.Pipeline;
using Core.Network;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Network;

public static class ServerBootstrap
{
    public static IChannel Channel { get; private set; } = null!;

    public static async void Initialize() {
        Console.WriteLine("Initializing netty bootstrap...");
        var bossGroup = new MultithreadEventLoopGroup(1);
        var workerGroup = new MultithreadEventLoopGroup();

        var bootstrap =  new NettyBootstrap.ServerBootstrap()
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 128)
            .Option(ChannelOption.TcpNodelay, true)
            .Option(ChannelOption.SoKeepalive, true)
            .ChildOption(ChannelOption.TcpNodelay, true)
            .ChildOption(ChannelOption.SoKeepalive, true)
            .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel => {
                var connection = new ClientConnection(NetworkSide.Server);
                connection.Listener = new ServerLoginHandler(connection);
                channel.Pipeline
                    // Here will be the packet decryptor
                    .AddLast("decoder", new PacketDecoder(NetworkSide.Client))
                    // Here will be the packet encryptor
                    .AddLast("encoder", new PacketEncoder(NetworkSide.Server))
                    .AddLast("handler", connection);
            }));

        Channel = await bootstrap.BindAsync(ServerNetwork.Port);

        Console.WriteLine($"Server listening on {ServerNetwork.Port}");

        await Task.Delay(-1, ServerNetwork.CancellationToken);

        Console.WriteLine("Shutting down server...");

        await Task.WhenAll(
            bossGroup.ShutdownGracefullyAsync(),
            workerGroup.ShutdownGracefullyAsync());
    }
}
