using System;
using System.Security.Cryptography;
using Client.Services;
using Client.ViewModel;
using Core.Network;
using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;
using Core.Network.Packets.S2C;

namespace Client.Network;

public class ClientHandshakeHandler : IClientHandshakeListener
{
    public ClientHandshakeHandler(ClientConnection ctx, ConnectionViewModel vm)
    {
        Ctx = ctx;
        Vm = vm;
    }

    private ConnectionViewModel Vm { get; }
    private ClientConnection Ctx { get; }
    private Aes? KeyPair { get; set; }

    /// <summary>
    ///     Called when the server responds with their capabilities.
    ///     These are then processed and settings are changed accordingly.
    /// </summary>
    /// <param name="packet">The received packet containing the server capabilities.</param>
    public async void OnHello(HelloS2CPacket packet)
    {
        Vm.HelloS2C.Complete();
        Vm.Capabilities = packet.Capabilities;
        if (packet.Capabilities.HasFlag(Capabilities.Ssl))
        {
            Vm.Add(Vm.EstablishSsl);
            await Ctx.EnableSecureSocketLayer();
            Vm.EstablishSsl.Complete();
        }

        Vm.Add(Vm.RequestConnection);
        await Ctx.Send(new ConnectC2SPacket(NetworkService.ClientInfo));
        Vm.RequestConnection.Complete();
        Vm.Add(Vm.ReceivedPublicKey);
    }

    /// <summary>
    ///     Called when the server responds to initiate the handshake.
    /// </summary>
    /// <param name="packet">The received packet containing the server's public RSA key.</param>
    public async void OnConnect(ConnectS2CPacket packet)
    {
        Vm.ReceivedPublicKey.Complete();
        Vm.Add(Vm.GeneratingKey);
        using (KeyPair = Aes.Create())
        {
            KeyPair.Mode = CipherMode.CFB;
            KeyPair.Padding = PaddingMode.PKCS7;
            KeyPair.KeySize = 256;
            KeyPair.GenerateKey();
        }

        byte[] encryptedKey;
        byte[] encryptedIv;
        try
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(packet.Key, out _);
            encryptedKey = rsa.Encrypt(KeyPair.Key, RSAEncryptionPadding.Pkcs1);
            encryptedIv = rsa.Encrypt(KeyPair.IV, RSAEncryptionPadding.Pkcs1);
        }
        catch (CryptographicException e)
        {
            Console.WriteLine($"Error encrypting key: {e.Message}");
            return;
        }

        await Ctx.Send(new KeyC2SPacket(encryptedKey, encryptedIv));
        Vm.GeneratingKey.Complete();
        Vm.Add(Vm.Encrypting);
    }

    /// <summary>
    ///     Called when the handshake is complete.
    ///     Both parties now enable the RSA encryption by modifying the pipeline.
    /// </summary>
    /// <param name="packet">The received packet to acknowledge the request.</param>
    public void OnSuccess(SuccessS2CPacket packet)
    {
        Ctx.EnableEncryption(KeyPair!);
        Ctx.State = NetworkState.Auth;
        Ctx.Listener = new ClientAuthHandler(Ctx);
        Vm.Encrypting.Complete();
        Vm.Complete();
    }

    /// <summary>
    ///     Called when the server experiences an error.
    ///     The client is then automatically disconnected from the network.
    /// </summary>
    /// <param name="packet">The received packet containing information about the error.</param>
    public void OnError(ErrorS2CPacket packet)
    {
        Vm.Error(packet.Error);
    }

    public void OnPing(PingS2CPacket packet)
    {
    }
}