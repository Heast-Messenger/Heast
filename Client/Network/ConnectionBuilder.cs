using System.Threading;

namespace Client.Network;

public class ConnectionBuilder
{
    public static ConnectionBuilder Configure()
    {
        return new ConnectionBuilder();
    }

    public void Start(string[]? args = null)
    {
        ClientNetwork.Initialize();
    }

    public void StartInNewThread(string[]? args = null)
    {
        var thread = new Thread(() => Start(args))
        {
            Name = "Client Network"
        };
        thread.Start();
    }
}