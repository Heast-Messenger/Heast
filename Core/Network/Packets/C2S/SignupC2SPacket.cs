﻿using Core.Network.Listeners;

namespace Core.Network.Packets.C2S;

public class SignupC2SPacket : IPacket<IServerAuthListener>
{

	public SignupC2SPacket(string username, string email, string password)
	{
		Username = username;
		Email = email;
		Password = password;
	}

	public SignupC2SPacket(PacketBuf buf)
	{
		Username = buf.ReadString();
		Email = buf.ReadString();
		Password = buf.ReadString();
	}

	public string Username { get; }
	public string Email { get; }
	public string Password { get; }

	public void Write(PacketBuf buf)
	{
		buf.WriteString(Username);
		buf.WriteString(Email);
		buf.WriteString(Password);
	}

	public void Apply(IServerAuthListener listener)
	{
		listener.OnSignup(this);
	}
}