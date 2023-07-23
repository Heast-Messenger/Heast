using System.Security.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketEncryptor : MessageToByteEncoder<IByteBuffer>
{
    public PacketEncryptor(ICryptoTransform transform)
    {
        Transform = transform;
    }

    private ICryptoTransform Transform { get; }

    protected override void Encode(IChannelHandlerContext context, IByteBuffer message, IByteBuffer output)
    {
        var rb = message.ReadableBytes;
        var bytes = new byte[rb];
        message.ReadBytes(bytes);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, Transform, CryptoStreamMode.Write);
        cryptoStream.Write(bytes);
        cryptoStream.FlushFinalBlock();

        var encrypted = memoryStream.ToArray();
        output.WriteBytes(encrypted);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        context.DisconnectAsync();
        Console.WriteLine($"Client kicked: {exception.Message}");
    }
}