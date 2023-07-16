using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class PingS2CPacket : AbstractPacket
{
	public PingS2CPacket(long startMs)
	{
		StartMs = startMs;
	}

	public PingS2CPacket(PacketBuf buf)
	{
		StartMs = buf.ReadLong();
	}

	public long StartMs { get; }

	public override void Write(PacketBuf buf)
	{
		buf.WriteLong(StartMs);
	}

	public override void Apply(IPacketListener listener)
	{
		if (listener is IClientHandshakeListener handshakeListener)
		{
			handshakeListener.OnPing(this);
		}
	}
}