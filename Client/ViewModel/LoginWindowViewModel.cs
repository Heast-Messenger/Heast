using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Avalonia.Threading;
using Client.Model;
using Client.Services;
using Client.View.Content;
using Client.View.Content.Login;
using Core.Network;
using Core.Network.Packets.C2S;
using Core.Utility;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace Client.ViewModel;

public class LoginWindowViewModel : ViewModelBase
{
    private readonly DispatcherTimer _pingTimer;
    private ConnectionViewModel _connectionService;
    private LoginBase _content = new WelcomePanel();
    private string _customServerAddress = string.Empty;
    private string? _error = string.Empty;

    public LoginWindowViewModel(IServiceProvider serviceProvider, NetworkService networkService, ConnectionViewModel connectionService)
    {
        _connectionService = connectionService;
        _pingTimer = new DispatcherTimer(TimeSpan.FromSeconds(2), DispatcherPriority.Background, PingAllServers);
        ServiceProvider = serviceProvider;
        NetworkService = networkService;
    }

    private IServiceProvider ServiceProvider { get; }
    private NetworkService NetworkService { get; }

    public LoginBase Content
    {
        get => _content;
        set
        {
            RaiseAndSetIfChanged(ref _content, value);
            _pingTimer.Start();
        }
    }

    public string? Error
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
        get => _connectionService;
        private set => RaiseAndSetIfChanged(ref _connectionService, value);
    }

    public bool GuestAllowed => ConnectionViewModel.Capabilities.HasFlag(Capabilities.Guest);
    public bool LoginAllowed => ConnectionViewModel.Capabilities.HasFlag(Capabilities.Login);
    public bool SignupAllowed => ConnectionViewModel.Capabilities.HasFlag(Capabilities.Signup);

    public void Back()
    {
        if (Content.Back is not null)
        {
            Content = Content.Back;
        }
    }

    public async void Signup()
    {
        if (NetworkService.Ctx?.State == NetworkState.Auth)
        {
            await NetworkService.Ctx.Send(new SignupC2SPacket(
                SignupUsername, SignupEmail, SignupPassword));
        }
        else
        {
            // TODO: Replace with dynamic translations
            Error = "Wait for Heast services to connect...";
        }
    }

    public async void Reset()
    {
        if (NetworkService.Ctx?.State == NetworkState.Auth)
        {
            await NetworkService.Ctx.Send(new ResetC2SPacket(
                LoginUsernameOrEmail, LoginPassword));
        }
        else
        {
            Error = "Wait for Heast services to connect...";
        }
    }

    public async void Login()
    {
        if (NetworkService.Ctx?.State == NetworkState.Auth)
        {
            await NetworkService.Ctx.Send(new LoginC2SPacket(
                LoginUsernameOrEmail, LoginPassword));
        }
        else
        {
            Error = "Wait for Heast services to connect...";
        }
    }

    public async void Guest()
    {
        if (NetworkService.Ctx?.State == NetworkState.Auth)
        {
            await NetworkService.Ctx.Send(new GuestC2SPacket(
                GuestUsername));
        }
        else
        {
            Error = "Wait for Heast services to connect...";
        }
    }

    public void ConnectOfficial()
    {
        Error = Parse(NetworkService.DefaultHost, NetworkService.DefaultPort, out var ipv4);
        if (ipv4 is not null)
        {
            ConnectServer(ipv4, NetworkService.DefaultPort);
        }
    }

    public void ConnectCustom()
    {
        Validation.Split(CustomServerAddress, out var host, out var port);
        host ??= NetworkService.DefaultHost;
        port ??= NetworkService.DefaultPort;
        Error = Parse(host, (int)port, out var ipv4);
        if (ipv4 is not null)
        {
            ConnectServer(ipv4, (int)port);
        }
    }

    private async void ConnectServer(IPAddress h, int p)
    {
        Content = new ConnectPanel
        {
            DataContext = this
        };

        ConnectionViewModel = ServiceProvider.GetService<ConnectionViewModel>();
        await NetworkService.Connect(h, p, ConnectionViewModel);
    }

    public void AddServer()
    {
        Error = string.Empty;

        Validation.Split(CustomServerAddress, out var host, out var port);
        host ??= NetworkService.DefaultHost;
        port ??= NetworkService.DefaultPort;
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

        var customServer = new CustomServer
        {
            Host = host,
            Port = (int)port
        };

        CustomServers.Add(customServer);
        PingServer(customServer);
    }

    private async void PingServer(CustomServer server)
    {
        server.Error = Parse(server.Host, server.Port, out var ipv4);
        if (ipv4 is null)
        {
            return;
        }

        try
        {
            server.Ping = await NetworkService.Ping(ipv4, server.Port);
            server.Status = ServerStatus.Running;
        }
        catch (Exception e)
        {
            if (e is ConnectTimeoutException or ConnectException)
            {
                server.Status = ServerStatus.Closed;
                return;
            }

            throw;
        }
    }

    private void PingAllServers()
    {
        foreach (var server in CustomServers)
        {
            PingServer(server);
        }
    }

    private void PingAllServers(object? sender, EventArgs e)
    {
        if (Content is CustomServerPanel)
        {
            PingAllServers();
        }
    }

    private static string? Parse(string host, int port, out IPAddress? address)
    {
        address = null;
        if (!Validation.Validate(host, port, out var localhost, out var domain, out var ipv4))
        {
            return "Invalid server address";
        }

        if (localhost)
        {
            try
            {
                address = IPAddress.Parse("127.0.0.1");
            }
            catch (FormatException)
            {
                return "Invalid IPv4 address format";
            }
        }

        if (domain)
        {
            try
            {
                var entry = Dns.GetHostEntry(host);
                if (entry.AddressList.Length > 0)
                {
                    address = entry.AddressList[0];
                }
            }
            catch (SocketException)
            {
                return "The hostname could not be resolved";
            }
        }

        if (ipv4)
        {
            address = IPAddress.Parse(host);
        }

        return null;
    }
}