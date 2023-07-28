using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
        TaskCompletionSource = new TaskCompletionSource();
        Ctx = ctx;
        Vm = vm;
    }

    private ConnectionViewModel Vm { get; }
    private ClientConnection Ctx { get; }
    private Aes? KeyPair { get; set; }

    public TaskCompletionSource TaskCompletionSource { get; }

    /// <summary>
    ///     Called when the server responds with their capabilities.
    ///     These are then processed and settings are changed accordingly.
    /// </summary>
    /// <param name="packet">The received packet containing the server capabilities.</param>
    public async void OnHello(HelloS2CPacket packet)
    {
        if (packet.HasErrors())
        {
            Vm.HelloS2C.Fail();
            return;
        }

        Vm.HelloS2C.Complete();

        Vm.Capabilities = packet.Capabilities;
        if (packet.Capabilities.HasFlag(Capabilities.Ssl))
        {
            Vm.Add(Vm.EstablishSsl);
            await Ctx.EnableSecureSocketLayer();
            Vm.EstablishSsl.Complete();
        }

        Vm.Add(Vm.RequestConnection);
        await Ctx.Send(new ConnectC2SPacket("Some client")); // TODO: Client info
        Vm.RequestConnection.Complete();
        Vm.Add(Vm.ReceivedPublicKey);
    }

    /// <summary>
    ///     Called when the server responds to initiate the handshake.
    /// </summary>
    /// <param name="packet">The received packet containing the server's public RSA key.</param>
    public async void OnConnect(ConnectS2CPacket packet)
    {
        if (packet.HasErrors())
        {
            Vm.ReceivedPublicKey.Fail();
            return;
        }

        Vm.ReceivedPublicKey.Complete();
        Vm.Add(Vm.GeneratingKey);
        using (KeyPair = Aes.Create())
        {
            KeyPair.Mode = CipherMode.CFB;
            KeyPair.Padding = PaddingMode.PKCS7;
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
    public void OnKey(KeyS2CPacket packet)
    {
        if (packet.HasErrors())
        {
            Vm.Encrypting.Fail();
            return;
        }

        Ctx.EnableEncryption(KeyPair!);
        TaskCompletionSource.SetResult();
        Vm.Encrypting.Complete();
        Vm.Complete();
    }

    public void OnPing(PingS2CPacket packet)
    {
        if (packet.HasErrors())
        {
        }
    }
}