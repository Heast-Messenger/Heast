using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class SuccessS2CPacket : IPacket
{

	public SuccessS2CPacket() { }

	public SuccessS2CPacket(PacketBuf buf) { }

	public void Write(PacketBuf buf) { }

	public void Apply(IPacketListener listener)
	{
		((IClientHandshakeListener)listener).OnSuccess(this);
	}
}

