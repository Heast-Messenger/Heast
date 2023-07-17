using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketDecoder : ReplayingDecoder
{
    public PacketDecoder(NetworkSide side)
    {
        Side = side;
    }

    public NetworkSide Side { get; }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object?> output)
    {
        var buffer = new PacketBuf(input);
        var id = buffer.ReadVarInt();
        var guid = buffer.ReadGuid();

        var packet = context.Channel
            .GetAttribute(ClientConnection.ProtocolKey).Get()
            .GetPacketHandler(Side)
            .CreatePacket(id, buffer);

        if (packet == null)
        {
            throw new InvalidDataException($"Bad packet id ({id})");
        }

        packet.Guid = guid;

        output.Add(packet);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
    {
        Console.WriteLine($"Exception whilst decoding: {exception.Message}");
    }
}