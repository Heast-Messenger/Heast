using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using Client.Converter;
using Client.Model;
using Client.Network;
using Client.View.Content;
using Core.Network;
using Core.Network.Packets.C2S;
using Core.Network.Pipeline;
using Core.Utility;
using static Client.Hooks;

namespace Client.ViewModel;

public class LoginWindowViewModel : ViewModelBase
{
	private LoginBase _content;
	private string _customServerAddress = string.Empty;
	private string _error = string.Empty;
	private Size _velocity;
	private Size _windowSize = new(400.0, 660.0);

	public LoginWindowViewModel()
	{
		_content = new WelcomePanel();
		DispatcherTimer resizeTimer = new();
		resizeTimer.Interval = TimeSpan.FromMilliseconds(1);
		resizeTimer.Tick += Resize;
		resizeTimer.Start();
	}

	private static Func<ClientConnection?> Connection => UseNetworking();

	private static Func<Window> Window => UseCurrentWindow();

	public LoginBase Content
	{
		get => _content;
		set
		{
			_windowSize = value.WindowSize ?? new(400.0, 660.0);
			RaiseAndSetIfChanged(ref _content, value, nameof(Content));
		}
	}

	public string Version => App.Version;

	public string Error
	{
		get => _error;
		set => RaiseAndSetIfChanged(ref _error, value, nameof(Error));
	}

	public string SignupUsername { get; set; } = string.Empty;
	public string SignupEmail { get; set; } = string.Empty;
	public string SignupPassword { get; set; } = string.Empty;
	public string LoginUsernameOrEmail { get; set; } = string.Empty;
	public string LoginPassword { get; set; } = string.Empty;
	public string GuestUsername { get; set; } = string.Empty;

	public string CustomServerAddress
	{
		get => _customServerAddress;
		set => RaiseAndSetIfChanged(ref _customServerAddress, value, nameof(CustomServerAddress));
	}

	public ObservableCollection<CustomServer> CustomServers { get; set; } = new();

	public void Resize(object? sender, EventArgs args)
	{
		var window = Window();
		if (window.PlatformImpl is null) return;

		var from = window.PlatformImpl.FrameSize!.Value;
		var to = _windowSize;
		var diff = to - from;
		if (Math.Abs(diff.Width) > 0.1 || Math.Abs(diff.Height) > 0.1)
		{
			var val = SmoothDamp.Read2d(from, to, ref _velocity, 1.0f, 0.1f);
			window.PlatformImpl.Resize(val, PlatformResizeReason.Application);
		}
	}

	public void Back()
	{
		if (Content.Back is not null)
		{
			Content = Content.Back;
		}
	}

	public async void ConnectOfficial()
	{
		try
		{
			Error = string.Empty;
			const string domain = "heast.ddns.net";
			const int port = 23010;
			var host = await ClientNetwork.Resolve(domain);
			ClientNetwork.Connect(host, port);
		}
		catch (Exception e)
		{
			Error = e.Message;
		}
	}

	public async void Signup()
	{
		var connection = Connection();
		if (connection?.State == NetworkState.Auth)
		{
			await connection.Send(new SignupC2SPacket(
				SignupUsername, SignupEmail, SignupPassword));
		}
		else
		{
			Error = "Wait for Heast services to connect...";
		}
	}

	public async void Reset()
	{
		var connection = Connection();
		if (connection?.State == NetworkState.Auth)
		{
			await connection.Send(new ResetC2SPacket(
				LoginUsernameOrEmail, LoginPassword));
		}
		else
		{
			Error = "Wait for Heast services to connect...";
		}
	}

	public async void Login()
	{
		var connection = Connection();
		if (connection?.State == NetworkState.Auth)
		{
			await connection.Send(new LoginC2SPacket(
				LoginUsernameOrEmail, LoginPassword));
		}
		else
		{
			Error = "Wait for Heast services to connect...";
		}
	}

	public async void Guest()
	{
		var connection = Connection();
		if (connection?.State == NetworkState.Auth)
		{
			await connection.Send(new GuestC2SPacket(
				GuestUsername));
		}
		else
		{
			Error = "Wait for Heast services to connect...";
		}
	}

	public async void Connect()
	{
		try
		{
			Error = string.Empty;
			var isIpv4 = Validation.IsIpv4(CustomServerAddress);
			var isDomain = Validation.IsDomain(CustomServerAddress);
			if (isIpv4 && CustomServerAddress.Split(":").Length == 2)
			{
				var host = CustomServerAddress.Split(":")[0];
				var port = int.Parse(CustomServerAddress.Split(":")[1]);
				ClientNetwork.Connect(host, port);
			}
			else if (isDomain)
			{
				var domain = CustomServerAddress.Split(":")[0];
				var host = await ClientNetwork.Resolve(domain);
				var port = 23010;
				if (CustomServerAddress.Split(":").Length == 2)
				{
					port = int.Parse(CustomServerAddress.Split(":")[1]);
				}
				ClientNetwork.Connect(host, port);
			}
			else
			{
				throw new("Invalid server address");
			}
		}
		catch (Exception e)
		{
			Error = e.Message;
		}
	}

	public void AddServer()
	{
		Error = string.Empty;
		var isIpv4 = Validation.IsIpv4(CustomServerAddress);
		var isDomain = Validation.IsDomain(CustomServerAddress);
		if (isIpv4 || isDomain)
		{
			var server = new CustomServer
			{
				Address = CustomServerAddress
			};
			CustomServers.Add(server);
		}
		else
		{
			Error = "Invalid server address";
		}
	}
}