using Core.Exceptions;
using Core.Network.Codecs;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;

namespace Core.Network.Pipeline; 

public class PacketDecoder : ReplayingDecoder<IPacket<IPacketListener>> {
    
    public NetworkSide Side { get; }

    public PacketDecoder(NetworkSide side) {
        this.Side = side;
    }

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object?> output) {
        var buffer = new PacketBuf(input);
        var id = buffer.ReadVarInt();

        var packet = context.Channel.GetAttribute(ClientConnection.ProtocolKey).Get()
            .GetPacketHandler(Side)
            .CreatePacket(id, buffer);

        if (packet == null) throw new InvalidDataException($"Bad packet id ({id})");
        output.Add(packet);
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception) {
        Console.WriteLine($"Exception whilst decoding: {exception.Message}");
    }
}