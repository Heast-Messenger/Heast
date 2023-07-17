using System;
using Client.Network;
using Core.Network.Codecs;

namespace Client;

public static partial class Hooks
{
    public static Func<ClientConnection?> UseNetworking()
    {
        return () => ClientNetwork.Ctx;
    }
}