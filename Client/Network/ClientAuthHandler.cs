using Core.Network.Listeners;
using Core.Network.Codecs;

namespace Client.Network;

public class ClientAuthHandler : IClientAuthListener
{
    public ClientAuthHandler(ClientConnection ctx)
    {
        Ctx = ctx;
    }

    private ClientConnection Ctx { get; }
}