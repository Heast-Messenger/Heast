using System;
using Avalonia;
using Core.Network.Pipeline;

namespace Client;

public static partial class Hooks
{
    public static (Func<ClientConnection> Get, Action<ClientConnection> Set) UseClientConnection()
    {
        App GetApp()
        {
            return (Application.Current as App)!;
        }

        return (
            () => GetApp().Ctx,
            v => GetApp().Ctx = v
        );
    }
}