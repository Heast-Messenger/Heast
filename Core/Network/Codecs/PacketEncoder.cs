using Core.exceptions;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace Core.Network.Codecs;

public class PacketEncoder : MessageToByteEncoder<AbstractPacket>
{
	public PacketEncoder(NetworkSide side)
	{
		Side = side;
	}

	public NetworkSide Side { get; }

	protected override void Encode(IChannelHandlerContext ctx, AbstractPacket message, IByteBuffer output)
	{
		var state = ctx.Channel.GetAttribute(ClientConnection.ProtocolKey).Get();
		var id = state.GetPacketId(Side, message);
		if (id != -1)
		{
			var buffer = new PacketBuf(output);
			buffer.WriteVarInt(id);
			buffer.WriteGuid(message.Guid);
			message.Write(buffer);
		}
		else
		{
			throw new IllegalStateException($"Bad packet id: {id}");
		}
	}

	public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
	{
		Console.WriteLine($"Exception whilst encoding: {exception.Message} ");
	}
}