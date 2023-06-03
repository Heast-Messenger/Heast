using Avalonia;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View.Content;

public partial class CustomServerPanel : LoginBase
{
	public CustomServerPanel()
	{
		InitializeComponent();
	}

	public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

	public override LoginBase Back => new ServerOptionsPanel
	{
		DataContext = DataContext
	};

	public override Size? WindowSize => new Size(500.0, 800.0);

	private void Button_OnConnect(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Connect();
	}

	private void Button_OnAdd(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.AddServer();
	}
}