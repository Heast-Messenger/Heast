using System;
using Core.Network.Pipeline;
using static Client.Hooks;

namespace Client.ViewModel.Content;

public class SettingsAccountViewModel : ContentViewModelBase
{
    private readonly (Func<ClientConnection> Get, Action<ClientConnection> Set) _ctx = UseClientConnection();

    public void Button_OnInvite()
    {
        Console.WriteLine("Invite");
    }

    public void Button_OnRequest()
    {
        Console.WriteLine("Request");
    }

    public void Button_OnSwitch()
    {
        Console.WriteLine("Switch");
    }

    public void Button_OnLogout()
    {
        Console.WriteLine("Logout");
    }

    public void Button_OnDelete()
    {
        Console.WriteLine("Delete");
    }
}