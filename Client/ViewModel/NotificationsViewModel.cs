using System.Collections.ObjectModel;
using Avalonia.Controls.Notifications;

namespace Client.ViewModel;

public class NotificationsViewModel : ViewModelBase
{
	public NotificationsViewModel()
	{
		Notifications = new()
		{
			new Notification("Title", "Message", NotificationType.Information),
			new Notification("Title", "Message", NotificationType.Warning),
			new Notification("Title", "Message", NotificationType.Error),
			new Notification("Title", "Message", NotificationType.Success)
		};
	}

	public ObservableCollection<INotification> Notifications { get; }

	public void Push(INotification notification)
	{
		Notifications.Add(notification);
	}

	public void Close(INotification notification)
	{
		Notifications.Remove(notification);
	}
}