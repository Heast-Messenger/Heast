using System;
using Client.Network;
using Core.Network.Pipeline;

namespace Client;

public static partial class Hooks
{
    public static (Func<ClientConnection> Get, Action<ClientConnection> Set) UseNetworking()
    {
        return (
            () => ClientNetwork.Ctx,
            v => ClientNetwork.Ctx = v
        );
    }
}