using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View;

public partial class LoginWindow : Window
{
	public LoginWindow()
	{
		InitializeComponent();
	}

	public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

	private void BackButton_OnClick(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Back();
	}
}