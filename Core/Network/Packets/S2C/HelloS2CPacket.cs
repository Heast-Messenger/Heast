using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class HelloS2CPacket : AbstractPacket
{
	public HelloS2CPacket(Capabilities capabilities)
	{
		Capabilities = capabilities;
	}

	public HelloS2CPacket(PacketBuf buf)
	{
		Capabilities = buf.ReadEnum<Capabilities>();
	}

	public Capabilities Capabilities { get; }

	public override void Write(PacketBuf buf)
	{
		buf.WriteEnum(Capabilities);
	}

	public override void Apply(IPacketListener listener)
	{
		if (listener is IClientHandshakeListener handshakeListener)
		{
			handshakeListener.OnHello(this);
		}
	}
}