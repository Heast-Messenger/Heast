using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View;

public partial class LoginWindow : Window
{
	public LoginWindow()
	{
		InitializeComponent();
		BackButton_OnLoaded();
	}

	public LoginWindowViewModel LoginWindowViewModel => (DataContext as LoginWindowViewModel)!;

	private void BackButton_OnClick(object? sender, RoutedEventArgs e)
	{
		LoginWindowViewModel.Back();
	}

	private void BackButton_OnLoaded()
	{
		if (OperatingSystem.IsWindows())
		{
			BackButton.SetValue(Grid.ColumnProperty, 0);
		}
		else if (OperatingSystem.IsMacOS())
		{
			BackButton.SetValue(Grid.ColumnProperty, 2);
		}
	}
}