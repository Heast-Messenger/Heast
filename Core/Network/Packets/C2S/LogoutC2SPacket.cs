using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class LogoutC2SPacket : IPacket
{
	public enum LogoutReason
	{
		Logout, Quit, Kick, Ban
	}

	public LogoutC2SPacket(LogoutReason reason)
	{
		Reason = reason;
	}

	public LogoutC2SPacket(PacketBuf buf)
	{
		Reason = buf.ReadEnum<LogoutReason>();
	}

	public LogoutReason Reason { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteEnum(Reason);
	}

	public void Apply(IPacketListener listener)
	{
		if (listener is IServerAuthListener authListener)
		{
			authListener.OnLogout(this);
		}
	}
}