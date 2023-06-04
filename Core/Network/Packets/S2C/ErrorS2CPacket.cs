using Core.Network.Listeners;

namespace Core.Network.Packets.S2C;

public class ErrorS2CPacket : IPacket<IClientLoginListener>
{
	public Error Error { get; }

    public ErrorS2CPacket(Error error)
    {
        Error = error;
    }

    public ErrorS2CPacket(PacketBuf buf)
    {
        Error = buf.ReadEnum<Error>();
    }

    public void Write(PacketBuf buf)
    {
        buf.WriteEnum(Error);
    }

    public void Apply(IClientLoginListener listener)
    {
        listener.OnError(this);
    }
}