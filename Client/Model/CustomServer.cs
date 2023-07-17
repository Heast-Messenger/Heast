using System;
using Avalonia.Media;
using Client.ViewModel;

namespace Client.Model;

public class CustomServer : ViewModelBase
{
	private string? _description;
	private string? _error;
	private string? _name;
	private long _ping;
	private ServerStatus? _status = ServerStatus.Pending;

	public string? Name
	{
		get => _name;
		set => RaiseAndSetIfChanged(ref _name, value);
	}

	public ServerStatus? Status
	{
		get => _status;
		set => RaiseAndSetIfChanged(ref _status, value);
	}

	public string Address
	{
		get => $"{Host}:{Port}";
		set
		{
			Host = value.Split(":")[0];
			Port = int.Parse(value.Split(":")[1]);
		}
	}

	public required string Host { get; set; }
	public required int Port { get; set; }

	public string? Description
	{
		get => _description;
		set => RaiseAndSetIfChanged(ref _description, value);
	}

	public string? Error
	{
		get => _error;
		set => RaiseAndSetIfChanged(ref _error, value);
	}

	public long Ping
	{
		get => _ping;
		set => RaiseAndSetIfChanged(ref _ping, value);
	}

	public IObservable<IImage>? Image { get; set; }
}