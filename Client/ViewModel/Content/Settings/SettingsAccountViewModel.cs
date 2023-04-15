using System;
using Core.Network.Pipeline;

namespace Client.ViewModel.Content;

public class SettingsAccountViewModel : ContentViewModelBase
{
    private readonly MainWindowViewModel _mainWindowVm;

    public SettingsAccountViewModel(MainWindowViewModel mainWindowVm)
    {
        _mainWindowVm = mainWindowVm;
    }

    private ClientConnection Ctx => _mainWindowVm.Ctx;

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