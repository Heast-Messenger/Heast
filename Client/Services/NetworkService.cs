using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Client.Network;
using Client.ViewModel;
using Core.Network.Codecs;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;
using Core.Utility;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Services;

public class NetworkService
{
    public NetworkService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public string DefaultHost { get; } = Shared.Config["default-host"]!;
    public int DefaultPort { get; } = int.Parse(Shared.Config["default-port"]!);
    public ClientConnection? Connection { get; set; }
    public ConcurrentQueue<IJob> ActionQueue { get; } = new();

    public void Initialize()
    {
        Console.WriteLine("Initializing client network...");

        while (true)
        {
            if (ActionQueue.TryDequeue(out var action))
            {
                try
                {
                    action.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while executing network action: {ex.Message}");
                }
            }

            // We don't wanna cook eggs with our CPU
            Thread.Sleep(0);
        }
    }

    public Task<TResult> RunAsync<TResult>(Func<TResult> function)
    {
        var job = new JobWithResult<TResult>(function);
        ActionQueue.Enqueue(job);
        return job.Task;
    }

    public void Post(Action action)
    {
        var job = new Job(action);
        ActionQueue.Enqueue(job);
    }

    public Task Connect(IPAddress host, int port, IServiceScope scope)
    {
        return RunAsync(async () =>
        {
            Console.WriteLine($"Connecting to {host}:{port}...");
            var vm = scope.ServiceProvider.GetRequiredService<ConnectionViewModel>();
            vm.Add(vm.HelloC2S);

            Connection = await ConnectInternal(host, port, scope);
            await Connection.Send(new HelloC2SPacket());

            vm.HelloC2S.Complete();
            vm.Add(vm.HelloS2C);
        });
    }

    private async Task<ClientConnection> ConnectInternal(IPAddress host, int port, IServiceScope scope)
    {
        // warning! this may become an issue later, when the NetworkService (singleton)
        //  is used twice to connect to different servers.
        // Because ClientConnection is Scoped to a singleton, it's automatically also a
        //  singleton and collides with further connections.
        // Solution: Create either a scope for the NetworkService or ServerConnect().
        var connection = scope.ServiceProvider.GetRequiredService<ClientConnection>();
        var workerGroup = new MultithreadEventLoopGroup();

        await new Bootstrap()
            .Group(workerGroup)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ClientConnectionInitializer(connection, scope))
            .ConnectAsync(host, port);

        return connection;
    }

    public async Task<long> Ping(IPAddress host, int port)
    {
        using (Connection = await ConnectInternal(host, port, ServiceProvider.CreateScope()))
        {
            Connection.Listener = ServiceProvider.GetRequiredService<ClientHandshakeHandler>();
            var oldMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            await Connection.SendAndWait<PingS2CPacket>(new PingC2SPacket(oldMs));
            var newMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return newMs - oldMs;
        }
    }
}