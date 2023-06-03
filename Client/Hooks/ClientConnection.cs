using Client.Network;
using Core.Network.Pipeline;

namespace Client;

public static partial class Hooks
{
	public static ClientConnection UseNetworking()
	{
		return ClientNetwork.Ctx!;
	}
}