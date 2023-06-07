using System;
using System.Threading;

namespace Client.Network;

public class ConnectionBuilder
{
	public static ConnectionBuilder Configure()
	{
		return new ConnectionBuilder();
	}

	public void Start(string[] args)
	{
		ClientNetwork.Initialize(args);
	}

	public void StartInNewThread(string[]? args = null)
	{
		var thread = new Thread(() => Start(args ?? Array.Empty<string>()))
		{
			Name = "Client Network"
		};
		thread.Start();
	}
}