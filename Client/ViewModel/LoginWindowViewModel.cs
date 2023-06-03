using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using Client.Converter;
using Client.Model;
using Client.View.Content;
using static Client.Hooks;

namespace Client.ViewModel;

public class LoginWindowViewModel : ViewModelBase
{
	private readonly DispatcherTimer _resizeTimer;
	private LoginBase _content;
	private double _velocity;
	private double _windowHeight = 600.0;

	public LoginWindowViewModel()
	{
		_content = new WelcomePanel();
		_resizeTimer = new DispatcherTimer();
		_resizeTimer.Interval = TimeSpan.FromTicks(1);
		_resizeTimer.Tick += Resize;
		_resizeTimer.Start();
	}

	private static Window CurrentWindow => UseCurrentWindow();

	public LoginBase Content
	{
		get => _content;
		set
		{
			_windowHeight = value.WindowHeight ?? 600.0;
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

	public ObservableCollection<CustomServer> CustomServers { get; set; } = new()
	{
		new CustomServer
		{
			Id = 1,
			Name = "Test Server",
			Address = "localhost:1234",
			Description = "This is a test server."
		}
	};

	public void Resize(object? sender, EventArgs args)
	{
		var from = CurrentWindow.Height;
		var to = _windowHeight;
		var diff = to - from;
		if (Math.Abs(diff) > 1.0f)
		{
			var val = SmoothDamp.Read(from, to, ref _velocity, 5.0f, 0.1f);
			CurrentWindow.Height = val;
		}
	}

	public void Back()
	{
		if (Content.Back is not null)
		{
			Content = Content.Back;
		}
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

	public void Connect()
	{
	}

	public void AddServer()
	{
	}
}