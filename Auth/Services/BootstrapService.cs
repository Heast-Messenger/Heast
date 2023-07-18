using Auth.Network;
using Core.Network;
using Core.Network.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using static System.Console;

namespace Auth.Services;

public class BootstrapService
{
    public BootstrapService(NetworkService networkService)
    {
        NetworkService = networkService;
    }

    private NetworkService NetworkService { get; }

    public IChannel Channel { get; private set; } = null!;

    public async void Initialize()
    {
        WriteLine("Initializing netty bootstrap...");
        var bossGroup = new MultithreadEventLoopGroup(1);
        var workerGroup = new MultithreadEventLoopGroup();

        Channel = await new ServerBootstrap()
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 128)
            .ChildHandler(new ClientHandler(NetworkService))
            .BindAsync(NetworkService.Port);

        WriteLine($"Server listening on {NetworkService.Port}");

        await Task.Delay(-1, NetworkService.CancellationToken);

        WriteLine("Shutting down server...");

        await Task.WhenAll(
            bossGroup.ShutdownGracefullyAsync(),
            workerGroup.ShutdownGracefullyAsync());
    }
}

public class ClientHandler : ChannelInitializer<ISocketChannel>
{
    public ClientHandler(NetworkService networkService)
    {
        NetworkService = networkService;
    }

    private NetworkService NetworkService { get; }

    protected override void InitChannel(ISocketChannel channel)
    {
        var connection = new ClientConnection(NetworkSide.Server);
        connection.Listener = new ServerHandshakeHandler(connection, NetworkService);
        connection.EnablePacketHandling(channel, connection);
    }
}