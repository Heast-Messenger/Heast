using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Client.Model;
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

	public override Size? WindowSize => new Size(500.0, 860.0);

	private void Button_OnConnect(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Connect();
	}

	private void Button_OnAdd(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.AddServer();
	}

	private void CustomServer_OnClick(object? sender, PointerPressedEventArgs e)
	{
		if (e.ClickCount == 1)
		{
			var server = ((sender as Control)!.DataContext as CustomServer)!;
			LoginWindowViewModel.CustomServerAddress = server.Address;
		}

		if (e.ClickCount == 2)
		{
			LoginWindowViewModel.Connect();
		}
	}
}