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

namespace Client.Services;

public class NetworkService
{
    public string DefaultHost { get; } = Shared.Config["default-host"]!;
    public int DefaultPort { get; } = int.Parse(Shared.Config["default-port"]!);
    public ClientConnection? Ctx { get; set; }
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

    public Task Connect(IPAddress host, int port, ConnectionViewModel vm)
    {
        return RunAsync(async () =>
        {
            Console.WriteLine($"Connecting to {host}:{port}...");
            vm.Add(vm.HelloC2S);

            Ctx = await ClientConnection.ServerConnect(host, port);
            Ctx.Listener = new ClientHandshakeHandler(Ctx, vm);

            await Ctx.Send(new HelloC2SPacket());
            vm.HelloC2S.Complete();
            vm.Add(vm.HelloS2C);
        });
    }

    public async Task<long> Ping(IPAddress host, int port)
    {
        using (Ctx = await ClientConnection.ServerConnect(host, port))
        {
            Ctx.Listener = new ClientHandshakeHandler(Ctx, null!);
            var oldMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            await Ctx.SendAndWait<PingS2CPacket>(new PingC2SPacket(oldMs));
            var newMs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return newMs - oldMs;
        }
    }
}