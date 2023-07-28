using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Core.Network;
using Core.Utility;
using static System.Console;

namespace Auth.Services;

public class NetworkService
{
    public int Port { get; set; } = 23010;
    public CancellationToken CancellationToken { get; } = new();
    public Capabilities Capabilities { get; private set; } = Capabilities.None;
    public RSACryptoServiceProvider KeyPair { get; } = new(dwKeySize: 4096);
    public X509Certificate2? Certificate { get; set; }

    public void Initialize()
    {
        if (Certificate is not null)
        {
            Capabilities |= Capabilities.Ssl;
        }

        {
            Capabilities |= Capabilities.Signup |
                            Capabilities.Login |
                            Capabilities.Guest;
        }
    }

    public void SetCertificate(string filepath)
    {
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