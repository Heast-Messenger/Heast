using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View.Content;

public partial class WelcomePanel : LoginBase
{
	public WelcomePanel()
	{
		InitializeComponent();
	}

	public override LoginBase? Back => null;

	public override double? WindowHeight => null;

	public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

	private void Button_OnClick(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Content = new ServerOptionsPanel
		{
			DataContext = DataContext
		};
	}
}