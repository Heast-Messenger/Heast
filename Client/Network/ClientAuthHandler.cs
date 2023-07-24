using System;
using System.Threading.Tasks;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.S2C;

namespace Client.Network;

public class ClientAuthHandler : IClientAuthListener
{
    public ClientAuthHandler(ClientConnection ctx)
    {
        TaskCompletionSource = new TaskCompletionSource();
        Ctx = ctx;
    }

    private ClientConnection Ctx { get; }
    public TaskCompletionSource TaskCompletionSource { get; }

    public void OnError(ErrorS2CPacket packet)
    {
        throw new NotImplementedException();
    }
}