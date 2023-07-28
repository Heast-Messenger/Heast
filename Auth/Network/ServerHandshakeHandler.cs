﻿using System.Security.Cryptography;
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
        TaskCompletionSource = new TaskCompletionSource();
        Ctx = ctx;
        NetworkService = networkService;
    }

    private ClientConnection Ctx { get; }
    private NetworkService NetworkService { get; }
    private Aes? KeyPair { get; set; }
    public TaskCompletionSource TaskCompletionSource { get; }

    /// <summary>
    ///     Called when the client wants to connect to the server.
    ///     Processes capabilities and changes pipeline settings accordingly.
    /// </summary>
    /// <param name="packet">The received packet.</param>
    public async void OnHello(HelloC2SPacket packet)
    {
        var capabilities = NetworkService.Capabilities;
        await Ctx.Send(new HelloS2CPacket(capabilities), guid: packet.Guid);
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
        await Ctx.Send(new ConnectS2CPacket(publicKey), guid: packet.Guid);
    }

    /// <summary>
    ///     Called when the client sends their symmetric communication key.
    /// </summary>
    /// <param name="packet">The received packet containing the client's AES key.</param>
    public async void OnKey(KeyC2SPacket packet)
    {
        using (KeyPair = Aes.Create())
        {
            KeyPair.Mode = CipherMode.CFB;
            KeyPair.Padding = PaddingMode.PKCS7;
        }

        try
        {
            KeyPair.Key = NetworkService.KeyPair.Decrypt(packet.Key, RSAEncryptionPadding.Pkcs1);
            KeyPair.IV = NetworkService.KeyPair.Decrypt(packet.Iv, RSAEncryptionPadding.Pkcs1);
        }
        catch (CryptographicException)
        {
            await Ctx.Send(new KeyS2CPacket(), ErrorCodes.InvalidKey, packet.Guid);
            return;
        }

        await Ctx.Send(new KeyS2CPacket(), guid: packet.Guid);
        Ctx.EnableEncryption(KeyPair);
        TaskCompletionSource.SetResult();
    }

    public void OnPing(PingC2SPacket packet)
    {
        var startMs = packet.StartMs;
        Ctx.Send(new PingS2CPacket(startMs), guid: packet.Guid);
    }
}