using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client.Model;
using Client.Network;
using Client.Utility;
using Client.View.Content;
using Client.View.Content.Login;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Packets.C2S;
using Core.Utility;
using static Client.Hooks;

namespace Client.ViewModel;

public class LoginWindowViewModel : ViewModelBase
{
	private ConnectionViewModel _connectionViewModel = null!;
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
			_windowSize = value.WindowSize ?? new Size(400.0, 660.0);
			RaiseAndSetIfChanged(ref _content, value);
		}
	}

	public string? Version => App.Version;

	public string Error
	{
		get => _error;
		set => RaiseAndSetIfChanged(ref _error, value);
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
		set => RaiseAndSetIfChanged(ref _customServerAddress, value);
	}

	public ObservableCollection<CustomServer> CustomServers { get; set; } = new();

	public ConnectionViewModel ConnectionViewModel
	{
		get => _connectionViewModel;
		private set => RaiseAndSetIfChanged(ref _connectionViewModel, value);
	}

	public void Resize(object? sender, EventArgs args)
	{
		var window = Window();
		if (window.PlatformImpl is null)
		{
			return;
		}

		var from = window.FrameSize!.Value;
		var to = _windowSize;
		var diff = to - from;
		if (Math.Abs(diff.Width) > 0.1 || Math.Abs(diff.Height) > 0.1)
		{
			var val = SmoothDamp.Read2d(from, to, ref _velocity, 1.0f, 0.1f);
			// window.Size = val;
		}
	}

	public void Back()
	{
		if (Content.Back is not null)
		{
			Content = Content.Back;
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

	public async void ConnectOfficial()
	{
		Error = await ConnectInternal(ClientNetwork.DefaultHost, ClientNetwork.DefaultPort) ?? "";
	}

	public async void ConnectCustom()
	{
		Validation.Split(CustomServerAddress, out var host, out var port);
		host ??= ClientNetwork.DefaultHost;
		port ??= ClientNetwork.DefaultPort;
		Error = await ConnectInternal(host, (int)port) ?? "";
	}

	private async void ConnectServer(IPAddress h, int p)
	{
		Content = new ConnectPanel
		{
			DataContext = this
		};

		ConnectionViewModel = new ConnectionViewModel();
		await ClientNetwork.Connect(h, p, ConnectionViewModel);
	}

	public void AddServer()
	{
		Error = string.Empty;

		Validation.Split(CustomServerAddress, out var host, out var port);
		host ??= ClientNetwork.DefaultHost;
		port ??= ClientNetwork.DefaultPort;
		if (!Validation.Validate(host, (int)port, out _, out _, out _))
		{
			Error = "Invalid server address";
			return;
		}

		if (CustomServers.Any(x => x.Address == $"{host}:{port}"))
		{
			Error = "Server already added";
			return;
		}

		CustomServers.Add(new CustomServer
		{
			Address = $"{host}:{port}"
		});
	}

	private async Task<string?> ConnectInternal(string host, int port)
	{
		try
		{
			IPAddress address = null!;
			if (!Validation.Validate(host, port, out var localhost, out var domain, out var ipv4))
			{
				throw new Exception("Invalid server address");
			}

			if (localhost)
			{
				address = IPAddress.Parse("127.0.0.1");
			}

			if (domain)
			{
				var entry = await Dns.GetHostEntryAsync(host);
				if (entry.AddressList.Length <= 0)
				{
					throw new Exception("Hostname could not be resolved");
				}

				address = entry.AddressList[0];
			}

			if (ipv4)
			{
				address = IPAddress.Parse(host);
			}

			ConnectServer(address, port);
		}
		catch (Exception e)
		{
			return e.Message;
		}

		return null;
	}
}