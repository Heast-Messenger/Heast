﻿using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class ResetC2SPacket : IPacket<IServerAuthListener>
{

	public ResetC2SPacket(string usernameOrEmail, string password)
	{
		UsernameOrEmail = usernameOrEmail;
		Password = password;
	}

	public ResetC2SPacket(PacketBuf buf)
	{
		UsernameOrEmail = buf.ReadString();
		Password = buf.ReadString();
	}

	public string UsernameOrEmail { get; }
	public string Password { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteString(UsernameOrEmail);
		buf.WriteString(Password);
	}

	public void Apply(IServerAuthListener listener)
	{
		listener.OnReset(this);
	}
}