using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class GuestC2SPacket : AbstractPacket
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

	public override void Write(PacketBuf buf)
	{
		buf.WriteString(Username);
	}

	public override void Apply(IPacketListener listener)
	{
		if (listener is IServerAuthListener authListener)
		{
			authListener.OnGuest(this);
		}
	}
}