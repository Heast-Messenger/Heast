using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Permissionengine;

namespace Chat.Model;
public class Client
{
    public int ClientId { get; set; }

    public Client()
    { }

    public bool HasPermission(int pid)
    {
        return PermissionsEngine.HasPermission(ClientId, pid);
    }

    public bool CanSeeChannel(int cid)
    {
        return PermissionsEngine.CanSeeChannel(cid, ClientId);
    }
    
    
}


