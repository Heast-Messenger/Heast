using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Client.ViewModel;

namespace Client.View;

public partial class Notifications : UserControl
{
	public Notifications()
	{
		InitializeComponent();
	}

	private NotificationsViewModel NotificationsViewModel => (NotificationsViewModel) DataContext!;

	public void CloseNotification(object? sender, RoutedEventArgs e)
	{
		if (sender is Button {DataContext: INotification notification})
			NotificationsViewModel.Close(notification);
	}
}