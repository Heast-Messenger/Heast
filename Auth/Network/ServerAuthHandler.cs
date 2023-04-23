using Core.Network.Listeners;
using Core.Network.Pipeline;

namespace Auth.Network;

public class ServerAuthHandler : IServerAuthListener
{
    public ServerAuthHandler(ClientConnection ctx)
    {
        Ctx = ctx;
    }

    private ClientConnection Ctx { get; }
}