using System.Security;
using System.Security.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketDecryptor : MessageToMessageDecoder<IByteBuffer>
{
    public PacketDecryptor(Aes keyPair)
    {
        KeyPair = keyPair;
    }

    private Aes KeyPair { get; }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
    {
        var rb = message.ReadableBytes;
        var bytes = new byte[rb];
        message.ReadBytes(bytes);

        var buf = context.Allocator.HeapBuffer();
        var decrypted = new Span<byte>();
        var success = KeyPair.TryDecryptCfb(
            bytes, KeyPair.IV, decrypted, out _, PaddingMode.PKCS7);
        if (!success) throw new SecurityException("Failed to decrypt packet!");

        buf.WriteBytes(decrypted.ToArray());
        output.Add(buf);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        context.DisconnectAsync();
        Console.WriteLine($"Client kicked: {exception.Message}");
    }
}