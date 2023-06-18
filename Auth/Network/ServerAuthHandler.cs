using Core.Network.Codecs;
using Core.Network.Listeners;
using Core.Network.Packets.C2S;

namespace Auth.Network;

public class ServerAuthHandler : IServerAuthListener
{
	public ServerAuthHandler(ClientConnection ctx)
	{
		Ctx = ctx;
	}

	private ClientConnection Ctx { get; }

	public void OnSignup(SignupC2SPacket packet)
	{
		throw new NotImplementedException();
	}

	public void OnLogin(LoginC2SPacket packet)
	{
		throw new NotImplementedException();
	}

	public void OnReset(ResetC2SPacket packet)
	{
		throw new NotImplementedException();
	}

	public void OnDelete(DeleteC2SPacket packet)
	{
		throw new NotImplementedException();
	}

	public void OnLogout()
	{
		throw new NotImplementedException();
	}

	public void OnVerify(VerifyC2SPacket packet)
	{
		throw new NotImplementedException();
	}

	public void OnGuest(GuestC2SPacket packet)
	{
		throw new NotImplementedException();
	}
}