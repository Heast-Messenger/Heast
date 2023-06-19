using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class VerifyC2SPacket : IPacket
{

	public VerifyC2SPacket(string code)
	{
		Code = code;
	}

	public VerifyC2SPacket(PacketBuf buf)
	{
		Code = buf.ReadString();
	}

	public string Code { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteString(Code);
	}
}