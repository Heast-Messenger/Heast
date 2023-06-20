using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class ErrorS2CPacket : IPacket
{

	public ErrorS2CPacket(Error error)
	{
		Error = error;
	}

	public ErrorS2CPacket(PacketBuf buf)
	{
		Error = buf.ReadEnum<Error>();
	}

	public Error Error { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteEnum(Error);
	}

	public void Apply(IPacketListener listener)
	{
		((IClientHandshakeListener)listener).OnError(this);
	}
}

