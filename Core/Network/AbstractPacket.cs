namespace Core.Network;

public abstract class AbstractPacket
{
    public Guid Guid { get; set; }
    public abstract void Write(PacketBuf buf);
    public abstract void Apply(IPacketListener listener);
}