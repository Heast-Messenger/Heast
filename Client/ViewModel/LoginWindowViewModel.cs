using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using Client.Converter;
using Client.Model;
using Client.View.Content;
using static Client.Hooks;

namespace Client.ViewModel;

public class LoginWindowViewModel : ViewModelBase
{
	private LoginBase _content;
	private Size _velocity;
	private Size _windowSize = new(400.0, 600.0);

	public LoginWindowViewModel()
	{
		_content = new WelcomePanel();
		DispatcherTimer resizeTimer = new();
		resizeTimer.Interval = TimeSpan.FromMilliseconds(1);
		resizeTimer.Tick += Resize;
		resizeTimer.Start();
	}

	private static Window CurrentWindow => UseCurrentWindow();

	public LoginBase Content
	{
		get => _content;
		set
		{
			_windowSize = value.WindowSize ?? new(400.0, 600.0);
			RaiseAndSetIfChanged(ref _content, value);
		}
	}

	public string SignupUsername { get; set; } = string.Empty;
	public string SignupEmail { get; set; } = string.Empty;
	public string SignupPassword { get; set; } = string.Empty;
	public string LoginUsernameOrEmail { get; set; } = string.Empty;
	public string LoginPassword { get; set; } = string.Empty;
	public string GuestUsername { get; set; } = string.Empty;
	public string CustomServerAddress { get; set; } = string.Empty;

	public ObservableCollection<CustomServer> CustomServers { get; set; } = new();

	public void Resize(object? sender, EventArgs args)
	{
		if (CurrentWindow.PlatformImpl is null) return;

		var from = CurrentWindow.PlatformImpl.FrameSize!.Value;
		var to = _windowSize;
		var diff = to - from;
		if (Math.Abs(diff.Width) > 0.1 || Math.Abs(diff.Height) > 0.1)
		{
			var val = SmoothDamp.Read2d(from, to, ref _velocity, 1.0f, 0.1f);
			CurrentWindow.PlatformImpl.Resize(val, PlatformResizeReason.Application);
		}
	}

	public void Back()
	{
		if (Content.Back is not null) Content = Content.Back;
	}

	public void Signup()
	{
		Console.WriteLine($"Signup: {SignupUsername} {SignupEmail} {SignupPassword}");
	}

	public void Reset()
	{
		Console.WriteLine($"Reset: {LoginUsernameOrEmail}");
	}

	public void Login()
	{
		Console.WriteLine($"Login: {LoginUsernameOrEmail} {LoginPassword}");
	}

	public void Guest()
	{
		Console.WriteLine($"Login: {GuestUsername}");
	}

	public void Connect() { }

	public void AddServer()
	{
		var server = new CustomServer
		{
			Address = CustomServerAddress
		};
		CustomServers.Add(server);
	}
}