using Avalonia;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View.Content;

public partial class LoginOptionsPanel : LoginBase
{
	public LoginOptionsPanel()
	{
		InitializeComponent();
	}

	public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

	public override LoginBase Back => new ServerOptionsPanel
	{
		DataContext = DataContext
	};

	public override Size? WindowSize => new Size(400.0, 670.0);

	private void Button_OnSignup(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Content = new SignupPanel
		{
			DataContext = DataContext
		};
	}

	private void Button_OnLogin(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Content = new LoginPanel
		{
			DataContext = DataContext
		};
	}

	private void Button_OnGuest(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Content = new GuestPanel
		{
			DataContext = DataContext
		};
	}
}