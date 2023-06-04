using System;
using Client.ViewModel;

namespace Client;

public static partial class Hooks
{
	public static Func<NotificationsViewModel> UseNotifications()
	{
		var mainVm = UseMainViewModel();
		return () => mainVm().NotificationsViewModel;
	}
}