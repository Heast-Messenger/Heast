using System;
using Client.Services;
using Core.Network.Codecs;

namespace Client;

public static partial class Hooks
{
    public static Func<ClientConnection?> UseNetworking()
    {
        return () => NetworkService.Ctx;
    }
}