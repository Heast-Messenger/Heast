using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class PingC2SPacket : AbstractPacket
{
	public PingC2SPacket(long startMs)
	{
		StartMs = startMs;
	}

	public PingC2SPacket(PacketBuf buf)
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
		if (listener is IServerHandshakeListener handshakeListener)
		{
			handshakeListener.OnPing(this);
		}
	}
}