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

        var encrypted = Encrypt(bytes);
        output.WriteBytes(encrypted);
    }

    private byte[] Encrypt(byte[] data)
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
        Console.WriteLine($"Exception whilst encrypting: {exception.Message}");
    }
}