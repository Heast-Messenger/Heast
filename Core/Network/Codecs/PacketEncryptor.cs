using System.Security;
using System.Security.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketEncryptor : MessageToByteEncoder<IByteBuffer>
{
    public PacketEncryptor(Aes keyPair)
    {
        KeyPair = keyPair;
    }

    private Aes KeyPair { get; }

    protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
    {
        var rb = message.ReadableBytes;
        var bytes = new byte[rb];
        message.ReadBytes(bytes);

        var encrypted = new Span<byte>();
        var success = KeyPair.TryEncryptCfb(
            bytes, KeyPair.IV, encrypted, out _, PaddingMode.PKCS7);
        if (!success) throw new SecurityException("Failed to encrypt packet!");
        output.WriteBytes(encrypted.ToArray());
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        context.DisconnectAsync();
        Console.WriteLine($"Client kicked: {exception.Message}");
    }
}