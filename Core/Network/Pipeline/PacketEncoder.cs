using System.Threading.Channels;
using Core.exceptions;
using DotNetty.Codecs;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;

namespace Core.Network.Pipeline; 

public class PacketEncoder : MessageToByteEncoder<IPacket<IPacketListener>> {
    
    public NetworkSide Side { get; }
    
    public PacketEncoder(NetworkSide side) {
        Side = side;
    }
    
    protected override void Encode(IChannelHandlerContext ctx, IPacket<IPacketListener> message, IByteBuffer output) {
        var state = ctx.Channel.GetAttribute(ClientConnection.ProtocolKey).Get();
        var id = state.GetPacketId(this.Side, message);
        if (id != -1) {
            var buffer = new PacketBuf(output);
            buffer.WriteVarInt(id);
            message.Write(buffer);
        }
        else {
            throw new IllegalStateException($"Bad packet id: {id}");
        }
    }

    public override void ExceptionCaught(IChannelHandlerContext context, Exception exception) {
        Console.WriteLine($"Exception whilst encoding: {exception.Message} ");
    }
}