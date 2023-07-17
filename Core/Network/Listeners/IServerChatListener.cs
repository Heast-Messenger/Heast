namespace Core.Network.Listeners;

public interface IServerChatListener : IPacketListener
{
    //void OnRequestPermission();
    void OnRequestUsers();
    void OnRequestChannels();
    void OnMessageSend();
}