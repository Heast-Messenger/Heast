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

	public required NetworkSide Target { get; init; } = NetworkSide.Client;
	public required string Title { get; init; } = string.Empty;
	public required string Description { get; init; } = string.Empty;
	public required string? Helplink { get; init; }

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