using System;

namespace Client.ViewModel.Content;

public class SettingsAccountViewModel : ContentViewModelBase
{
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