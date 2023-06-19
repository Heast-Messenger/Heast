using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class GuestC2SPacket : IPacket
{

	public GuestC2SPacket(string username)
	{
		Username = username;
	}

	public GuestC2SPacket(PacketBuf buf)
	{
		Username = buf.ReadString();
	}

	public string Username { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteString(Username);
	}
}