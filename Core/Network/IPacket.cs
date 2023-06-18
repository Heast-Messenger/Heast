namespace Core.Network;

public interface IPacket<in T> where T : IPacketListener
{
	void Write(PacketBuf buf);
	void Apply(T listener);
}