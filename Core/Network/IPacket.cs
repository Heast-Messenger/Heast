namespace Core.Network;

public interface IPacket
{
	void Write(PacketBuf buf);
}