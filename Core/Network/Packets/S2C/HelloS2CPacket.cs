using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class HelloS2CPacket : IPacket
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

	public void Write(PacketBuf buf)
	{
		buf.WriteEnum(Capabilities);
	}

	public void Apply(IPacketListener listener)
	{
		if (listener is IClientHandshakeListener handshakeListener)
		{
			handshakeListener.OnHello(this);
		}
	}
}