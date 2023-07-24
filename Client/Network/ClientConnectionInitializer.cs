using System.Threading.Tasks;
using Core.Network;
using Core.Network.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Network;

public class ClientConnectionInitializer : ChannelInitializer<ISocketChannel>
{
    public ClientConnectionInitializer(ClientConnection connection, IServiceScope serviceScope)
    {
        Connection = connection;
        ServiceScope = serviceScope;
    }

    private ClientConnection Connection { get; }
    private IServiceScope ServiceScope { get; }
    private TaskCompletionSource TaskCompletionSource => Connection.Listener.TaskCompletionSource;

    protected override async void InitChannel(ISocketChannel channel)
    {
        Connection.EnablePacketHandling(channel);

        do
        {
            Connection.Listener = ServiceScope.ServiceProvider.GetRequiredService<ClientHandshakeHandler>();
            await TaskCompletionSource.Task;

            if (TaskCompletionSource.Task.IsCanceled)
            {
                return;
            }

            Connection.State = NetworkState.Auth;
            Connection.Listener = ServiceScope.ServiceProvider.GetRequiredService<ClientAuthHandler>();
            await TaskCompletionSource.Task;

            if (TaskCompletionSource.Task.IsCompletedSuccessfully)
            {
            }
        } while (!TaskCompletionSource.Task.IsCompletedSuccessfully);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        TaskCompletionSource.SetCanceled();
        ServiceScope.Dispose();
    }
}