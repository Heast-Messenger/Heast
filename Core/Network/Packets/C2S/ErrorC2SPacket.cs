using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class ErrorC2SPacket : IPacket
{

	public ErrorC2SPacket(Error error, string message)
	{
		Error = error;
		Message = message;
	}

	public ErrorC2SPacket(PacketBuf buf)
	{
		Error = buf.ReadEnum<Error>();
		Message = buf.ReadString();
	}

	public Error Error { get; }
	public string Message { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteEnum(Error);
		buf.WriteString(Message);
	}
}