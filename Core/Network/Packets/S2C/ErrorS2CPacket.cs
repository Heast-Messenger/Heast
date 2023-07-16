using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class ErrorS2CPacket : AbstractPacket
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

	public override void Write(PacketBuf buf)
	{
		buf.WriteEnum(Error);
	}

	public override void Apply(IPacketListener listener)
	{
		if (listener is IClientHandshakeListener handshakeListener)
		{
			handshakeListener.OnError(this);
		}

		if (listener is IClientAuthListener authListener)
		{
			authListener.OnError(this);
		}
	}
}