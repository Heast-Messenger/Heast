using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Avalonia.Threading;
using Client.Model;
using Client.Services;
using Client.View.Content;
using Client.View.Content.Login;
using Client.View.Content.Modals;
using Core.Extensions;
using Core.Network;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;
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

    public LoginWindowViewModel(
        IServiceProvider serviceProvider,
        NetworkService networkService,
        ConnectionViewModel connectionService,
        ModalViewModel modalService)
    {
        _connectionService = connectionService;
        _pingTimer = new DispatcherTimer(TimeSpan.FromSeconds(value: 2),
            DispatcherPriority.Background,
            TryPingAllServers);
        ServiceProvider = serviceProvider;
        NetworkService = networkService;
        ModalService = modalService;
    }

    private IServiceProvider ServiceProvider { get; }
    private NetworkService NetworkService { get; }
    public ModalViewModel ModalService { get; }

    public LoginBase Content
    {
        get => _content;
        set
        {
            if (value.GetType() == _content.GetType())
            {
                return;
            }

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
    public string LoginEmail { get; set; } = string.Empty;
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

    /// <summary>
    ///     Verifies the signup code by sending a request to the server. The server checks the code by comparing the actual
    ///     code that is linked to the email address to the received code. The code is only valid for <b>up to 5 minutes</b>.
    ///     If the user exceeds this time frame, this function will 'not work' anymore and immediately return false.
    /// </summary>
    /// <param name="code">The code to check the actual code against</param>
    /// <param name="email">The email that the code is linked to</param>
    /// <param name="purpose">The purpose for which this email is being verified.</param>
    /// <returns>True, if the code matches the actual code. False otherwise.</returns>
    private async Task<bool> VerifySignupCode(string code, string email,
        VerifyEmailC2SPacket.VerificationPurpose purpose)
    {
        if (NetworkService.Connection?.State != NetworkState.Auth)
        {
            Error = "Wait for Heast services to connect...";
            return false;
        }

        var result = await NetworkService.Connection.SendAndWait<VerifyEmailS2CPacket>(
            new VerifyEmailC2SPacket(code, email, purpose));

        if (result.Status == VerifyEmailS2CPacket.ResponseStatus.WrongCode)
        {
            if (ModalService.Modal is EmailVerificationModal emailVerificationModal)
            {
                emailVerificationModal.WrongCode();
            }
        }

        else if (result.Status == VerifyEmailS2CPacket.ResponseStatus.Unauthorized)
        {
            Error = "Unauthorized.";
            ModalService.Close();
            return false;
        }

        else if (result.Status == VerifyEmailS2CPacket.ResponseStatus.Success)
        {
            ModalService.Close();
            return true;
        }

        return false;
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
        if (NetworkService.Connection?.State != NetworkState.Auth)
        {
            Error = "Wait for Heast services to connect...";
            return;
        }

        var result = await NetworkService.Connection.SendAndWait<SignupS2CPacket>(
            new SignupC2SPacket(SignupUsername, SignupEmail, SignupPassword));

        if (result.HasErrors())
        {
            Error = result.GetErrors();
        }

        if (result.Status == SignupS2CPacket.ResponseStatus.AwaitingConfirmation)
        {
            ModalService.Modal = new EmailVerificationModal(code =>
                VerifySignupCode(code, SignupEmail, VerifyEmailC2SPacket.VerificationPurpose.Signup).Run(success =>
                {
                    if (success)
                    {
                        Console.Out.WriteLine("Welcome new user!");
                        // Content = new TutorialPanel
                        // {
                        //     DataContext = this
                        // };
                    }
                    else
                    {
                        Content = new SignupPanel
                        {
                            DataContext = this
                        };
                    }
                }));
        }
    }

    public async void Login()
    {
        if (NetworkService.Connection?.State != NetworkState.Auth)
        {
            Error = "Wait for Heast services to connect...";
            return;
        }

        var result = await NetworkService.Connection.SendAndWait<LoginS2CPacket>(
            new LoginC2SPacket(LoginEmail, LoginPassword));

        if (result.HasErrors())
        {
            Error = result.GetErrors();
        }

        if (result.Status == LoginS2CPacket.ResponseStatus.AwaitingConfirmation)
        {
            ModalService.Modal = new EmailVerificationModal(code =>
                VerifySignupCode(code, LoginEmail, VerifyEmailC2SPacket.VerificationPurpose.Login).Run(success =>
                {
                    if (success)
                    {
                        Console.Out.WriteLine("Welcome back!");
                        // Content = new ClientLoader
                        // {
                        //     DataContext = this
                        // };
                    }
                    else
                    {
                        Content = new LoginPanel
                        {
                            DataContext = this
                        };
                    }
                }));
        }
    }

    public async void Reset()
    {
        // if (NetworkService.Connection?.State != NetworkState.Auth)
        // {
        //     Error = "Wait for Heast services to connect...";
        //     return;
        // }
        //
        // await NetworkService.Connection.Send(new ResetC2SPacket(
        //     LoginUsernameOrEmail,
        //     LoginPassword));
    }

    public async void Guest()
    {
        // if (NetworkService.Connection?.State != NetworkState.Auth)
        // {
        //     Error = "Wait for Heast services to connect...";
        //     return;
        // }
        //
        // await NetworkService.Connection.Send(new GuestC2SPacket(
        //     GuestUsername));
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

        var scope = ServiceProvider.CreateScope();
        {
            ConnectionViewModel = scope.ServiceProvider.GetRequiredService<ConnectionViewModel>();
            // TODO: Maybe make NetworkService scoped and require it here? Then I don't need to pass the scope around.
            await NetworkService.Connect(h, p, scope);
        }
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

    private void TryPingAllServers(object? sender, EventArgs e)
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