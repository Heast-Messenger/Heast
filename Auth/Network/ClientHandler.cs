using Core.Network;
using Core.Network.Codecs;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Network;

public class ClientHandler : ChannelInitializer<ISocketChannel>
{
    public ClientHandler(IServiceProvider serviceProvider)
    {
        ServiceScope = serviceProvider.CreateScope();
        Connection = ServiceScope.ServiceProvider.GetRequiredService<ClientConnection>();
    }

    private IServiceScope ServiceScope { get; }
    private ClientConnection Connection { get; }
    private TaskCompletionSource TaskCompletionSource => Connection.Listener.TaskCompletionSource;

    protected override async void InitChannel(ISocketChannel channel)
    {
        Connection.EnablePacketHandling(channel);

        do
        {
            Connection.Listener = ServiceScope.ServiceProvider.GetRequiredService<ServerHandshakeHandler>();
            await TaskCompletionSource.Task;

            if (TaskCompletionSource.Task.IsCanceled)
            {
                return;
            }

            Connection.State = NetworkState.Auth;
            Connection.Listener = ServiceScope.ServiceProvider.GetRequiredService<ServerAuthHandler>();
            await TaskCompletionSource.Task;

            if (TaskCompletionSource.Task.IsCompletedSuccessfully)
            {
            }
        } while (!TaskCompletionSource.Task.IsCompletedSuccessfully);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        TaskCompletionSource.SetCanceled();
    }
}