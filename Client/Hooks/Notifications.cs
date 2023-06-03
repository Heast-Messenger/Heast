using Client.ViewModel;

namespace Client;

public static partial class Hooks
{
    public static NotificationsViewModel UseNotifications()
    {
        var mw = UseMainWindow();

        return mw.NotificationsViewModel;
    }
}