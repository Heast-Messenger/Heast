namespace ChatServer.events;

public delegate void OnConnect(int id);

public class EventLogic
{
    public event OnConnect OnConnect;

    //TODO Invoke Event
    public void UserConnect(int id)
    {
        OnConnect?.Invoke(id);
    }
}