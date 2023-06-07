using System.Threading;

namespace Client.Network;

public class ConnectionBuilder
{
	public static ConnectionBuilder Configure()
	{
		return new();
	}

	public void Start(string[] args)
	{
		ClientNetwork.Initialize(args);
	}

	public void StartInNewThread(string[] args)
	{
		var thread = new Thread(() => Start(args))
		{
			Name = "Client Network"
		};
		thread.Start();
	}
}