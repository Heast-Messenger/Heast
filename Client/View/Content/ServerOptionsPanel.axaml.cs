using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View.Content;

public partial class ServerOptionsPanel : LoginBase
{
	public ServerOptionsPanel()
	{
		InitializeComponent();
	}

	public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

	public override LoginBase? Back => new WelcomePanel
	{
		DataContext = DataContext
	};

	public override double? WindowHeight => null!;

	private void Button_OnOfficial(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Content = new LoginOptionsPanel
		{
			DataContext = DataContext
		};
	}

	private void Button_OnCustom(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Content = new CustomServerPanel
		{
			DataContext = DataContext
		};
	}
}