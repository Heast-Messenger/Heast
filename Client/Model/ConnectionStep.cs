using Client.ViewModel;
using Core.Network;

namespace Client.Model;

public class ConnectionStep : ViewModelBase
{
	private ConnectionStatus _status = ConnectionStatus.Pending;

	public ConnectionStatus Status
	{
		get => _status;
		private set => RaiseAndSetIfChanged(ref _status, value);
	}

	public NetworkSide Target { get; init; } = NetworkSide.Client;
	public string Title { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public string Helplink { get; init; } = string.Empty;

	public void Complete()
	{
		if (Status == ConnectionStatus.Pending)
		{
			Status = ConnectionStatus.Successful;
		}
	}

	public void Fail()
	{
		if (Status == ConnectionStatus.Pending)
		{
			Status = ConnectionStatus.Failed;
		}
	}
}