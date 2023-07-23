using System.Security.Cryptography;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketDecryptor : MessageToMessageDecoder<IByteBuffer>
{
    public PacketDecryptor(ICryptoTransform transform)
    {
        Transform = transform;
    }

    private ICryptoTransform Transform { get; }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
    {
        var rb = message.ReadableBytes;
        var bytes = new byte[rb];
        message.ReadBytes(bytes);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, Transform, CryptoStreamMode.Write);
        cryptoStream.Write(bytes);
        cryptoStream.FlushFinalBlock();

        var buf = context.Allocator.HeapBuffer();
        var decrypted = memoryStream.ToArray();
        buf.WriteBytes(decrypted);
        output.Add(buf);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        context.DisconnectAsync();
        Console.WriteLine($"Client kicked: {exception.Message}");
    }
}