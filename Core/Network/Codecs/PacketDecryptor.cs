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

        var buf = context.Allocator.HeapBuffer();
        var decrypted = Decrypt(bytes);
        buf.WriteBytes(decrypted);
        output.Add(buf);
    }

    private byte[] Decrypt(byte[] data)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, Transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data);
            }

            return memoryStream.ToArray();
        }
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        context.DisconnectAsync();
        Console.WriteLine($"Exception whilst decrypting: {exception.Message}");
    }
}