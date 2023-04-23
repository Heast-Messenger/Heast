using Core.Network.Listeners;
using Core.Network.Pipeline;

namespace Client.Network;

public class ClientAuthHandler : IClientAuthListener
{
    public ClientAuthHandler(ClientConnection ctx)
    {
        Ctx = ctx;
    }

    private ClientConnection Ctx { get; }
}