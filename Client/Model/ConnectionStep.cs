using Core.Network;

namespace Client.Model;

public class ConnectionStep
{
	public NetworkSide Target { get; init; } = NetworkSide.Client;
	public string Title { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty!;
}

public static class ConnectionSteps
{
	public static ConnectionStep HelloC2S => new()
	{
		Target = NetworkSide.Server,
		Title = "Pinging Server",
		Description = "The first step of the HNEP (Heast Network Exchange Protocoll). " +
		              "Informs the server to get ready to connect."
	};

	public static ConnectionStep HelloS2C => new()
	{
		Target = NetworkSide.Client,
		Title = "Sending Capabilities",
		Description = "The server now sends their capabilities to the client. " +
		              "These include flags such as if SSL communication is supported."
	};
}