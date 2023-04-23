using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View;

public partial class Notifications : UserControl
{
	public Notifications()
	{
		this.InitializeComponent();
	}

	private NotificationsViewModel NotificationsViewModel => (NotificationsViewModel) this.DataContext!;

	public void CloseNotification(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.DataContext is INotification notification)
			this.NotificationsViewModel.Close(notification);
	}
}