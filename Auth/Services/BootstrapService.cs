using Auth.Network;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using static System.Console;

namespace Auth.Services;

public class BootstrapService
{
    public BootstrapService(IServiceProvider serviceProvider, NetworkService networkService)
    {
        ServiceProvider = serviceProvider;
        NetworkService = networkService;
    }

    private IServiceProvider ServiceProvider { get; }
    private NetworkService NetworkService { get; }
    private IChannel? Channel { get; set; }

    public async void Initialize()
    {
        WriteLine("Initializing netty bootstrap...");
        var bossGroup = new MultithreadEventLoopGroup(1);
        var workerGroup = new MultithreadEventLoopGroup();

        Channel = await new ServerBootstrap()
            .Group(bossGroup, workerGroup)
            .Channel<TcpServerSocketChannel>()
            .Option(ChannelOption.SoBacklog, 128)
            .ChildHandler(new ClientHandler(ServiceProvider))
            .BindAsync(NetworkService.Port);

        WriteLine($"Server listening on {NetworkService.Port}");

        await Task.Delay(-1, NetworkService.CancellationToken);

        WriteLine("Shutting down server...");

        await Task.WhenAll(
            bossGroup.ShutdownGracefullyAsync(),
            workerGroup.ShutdownGracefullyAsync());
    }
}