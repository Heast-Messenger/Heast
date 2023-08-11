using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Core.Network;
using Core.Services;
using Core.Utility;
using Microsoft.Extensions.Logging;
using static System.Console;

namespace Auth.Services;

public class NetworkService : IService
{
    public NetworkService(ILogger<NetworkService> logger)
    {
        Logger = logger;
    }

    private ILogger<NetworkService> Logger { get; }
    public int Port { get; set; } = 23010;
    public CancellationToken CancellationToken { get; } = new();
    public Capabilities Capabilities { get; private set; } = Capabilities.None;
    public RSACryptoServiceProvider KeyPair { get; } = new(dwKeySize: 4096);
    public X509Certificate2? Certificate { get; set; }

    public Task<bool> Initialize()
    {
        Logger.LogInformation("Initializing ServerNetwork");
        SetupCapabilities();
        return Task.FromResult(true);
    }

    private void SetupCapabilities()
    {
        if (Certificate is not null)
        {
            Capabilities |= Capabilities.Ssl;
        }

        Capabilities |= Capabilities.Signup;
        Capabilities |= Capabilities.Login;
        Capabilities |= Capabilities.Guest;
    }

    public void SetCertificate(string filepath)
    {
        Logger.LogInformation("Setting certificate '{}'", filepath);
        try
        {
            Certificate = new X509Certificate2(filepath, Shared.Config["ssh-password"]);
        }
        catch (CryptographicException e)
        {
            WriteLine($"Error creating SSL certificate: {e.Message}");
        }
    }
}