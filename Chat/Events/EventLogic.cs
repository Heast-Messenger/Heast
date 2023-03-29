namespace Chat.events;

public delegate void OnConnect(int id);

public delegate bool OnRolePermissionChanged(int userId);

public class EventLogic
{
    public event OnConnect OnConnect;
    public event OnRolePermissionChanged OnPermissionChanged;

    //TODO Invoke Event
    public void ClientConnect(int id)
    {
        OnConnect?.Invoke(id);
    }

    public void ClientUpdate(int roleId)
    {
        OnPermissionChanged?.Invoke(roleId);
    }
}