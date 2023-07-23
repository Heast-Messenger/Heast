using System.Security.Cryptography;
using Auth.Services;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Auth.Network;

public class ServerHandshakeHandler : IServerHandshakeListener
{
    public ServerHandshakeHandler(ClientConnection ctx, NetworkService networkService)
    {
        Ctx = ctx;
        NetworkService = networkService;
    }

    private ClientConnection Ctx { get; }
    private NetworkService NetworkService { get; }

    /// <summary>
    ///     Called when the client wants to connect to the server.
    ///     Processes capabilities and changes pipeline settings accordingly.
    /// </summary>
    /// <param name="packet">The received packet.</param>
    public async void OnHello(HelloC2SPacket packet)
    {
        var capabilities = NetworkService.Capabilities;
        await Ctx.Send(new HelloS2CPacket(capabilities), packet.Guid);
        if (capabilities.HasFlag(Capabilities.Ssl))
        {
            var certificate = NetworkService.Certificate;
            await Ctx.EnableSecureSocketLayer(certificate);
        }
    }

    /// <summary>
    ///     Called when the client is ready to connect to the server
    ///     and initiates the handshake.
    /// </summary>
    /// <param name="packet">The received packet containing client information.</param>
    public async void OnConnect(ConnectC2SPacket packet)
    {
        var publicKey = NetworkService.KeyPair.ExportRSAPublicKey();
        await Ctx.Send(new ConnectS2CPacket(publicKey), packet.Guid);
    }

    /// <summary>
    ///     Called when the client sends their symmetric communication key.
    /// </summary>
    /// <param name="packet">The received packet containing the client's AES key.</param>
    public async void OnKey(KeyC2SPacket packet)
    {
        using var keypair = Aes.Create();
        try
        {
            var key = NetworkService.KeyPair.Decrypt(packet.Key, RSAEncryptionPadding.Pkcs1);
            var iv = NetworkService.KeyPair.Decrypt(packet.Iv, RSAEncryptionPadding.Pkcs1);
            keypair.Mode = CipherMode.CFB;
            keypair.Padding = PaddingMode.PKCS7;
            keypair.Key = key;
            keypair.IV = iv;
        }
        catch (CryptographicException)
        {
            await Ctx.Send(new ErrorS2CPacket(Error.InvalidKey), packet.Guid);
            await NetworkService.Disconnect(Ctx);
            return;
        }

        await Ctx.Send(new SuccessS2CPacket());
        Ctx.EnableEncryption(keypair);
        Ctx.State = NetworkState.Auth;
        Ctx.Listener = new ServerAuthHandler(Ctx);
    }

    public void OnPing(PingC2SPacket packet)
    {
        var startMs = packet.StartMs;
        Ctx.Send(new PingS2CPacket(startMs), packet.Guid);
    }
}