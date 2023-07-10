using System.Collections.ObjectModel;
using System.Linq;
using Client.Model;
using Core.Network;

namespace Client.ViewModel;

public class ConnectionViewModel : ViewModelBase
{
	public ObservableCollection<ConnectionStep> ConnectionSteps { get; set; } = new();

	public ConnectionStep HelloC2S { get; } = new()
	{
		Target = NetworkSide.Server,
		Title = "Pinging Server",
		Description = "The first step of the HNEP (Heast Network Exchange Protocol). " +
		              "Informs the server to get ready to connect.",
		Helplink = "https://www.google.com"
	};

	public ConnectionStep HelloS2C { get; } = new()
	{
		Target = NetworkSide.Client,
		Title = "Receiving Capabilities",
		Description = "The server now sends their capabilities to the client. " +
		              "These include flags such as if SSL communication is supported."
	};

	public ConnectionStep EstablishSsl { get; } = new()
	{
		Target = NetworkSide.Client,
		Title = "Secure Socket Layer Detected",
		Description = "The server is capable of handling inbound SSL connections. " +
		              "The client will now try to establish an SSL connection."
	};

	public ConnectionStep RequestConnection { get; } = new()
	{
		Target = NetworkSide.Server,
		Title = "Sending Client Information",
		Description = "The client now sends their information to the server. " +
		              "The information includes cookies such as the UA (user-agent) and more."
	};

	public ConnectionStep ReceivedPublicKey { get; } = new()
	{
		Target = NetworkSide.Client,
		Title = "Received Public Key",
		Description = "The server sent its public key to the client. " +
		              "This key is now used to encrypt a shared, symmetric key."
	};

	public ConnectionStep GeneratingKey { get; } = new()
	{
		Target = NetworkSide.Server,
		Title = "Generating Symmetric Key",
		Description = "The client now generates a random symmetric key, " +
		              "encrypts it using the public key, and sends it off to the server."
	};

	public ConnectionStep Encrypting { get; } = new()
	{
		Target = NetworkSide.Client,
		Title = "Encrypting",
		Description = "If everything went right, the server responds with a success message, " +
		              "telling the client to finally enable the encryption handlers in the pipeline."
	};

	public void Add(ConnectionStep step)
	{
		if (!ConnectionSteps.Contains(step))
		{
			ConnectionSteps.Add(step);
		}
	}

	public void Error(Error error)
	{
		var step = ConnectionSteps.Last();
		step.Fail();
	}
}