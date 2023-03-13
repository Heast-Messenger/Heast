using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Permissionengine;

namespace Chat.Model;

[Table("permissionclients")]
public class PermissionClient
{
    [Key]
    public int PermissionClientId { get; set; }

    public PermissionClient(int permissionClientId)
    {
        PermissionClientId = permissionClientId;
    }

    public bool HasPermission(int pid)
    {
        return PermissionsEngine.HasPermission(PermissionClientId, pid);
    }

    public bool CanSeeChannel(int cid)
    {
        return PermissionsEngine.ChannelVisibleToClient(cid, PermissionClientId);
    }
    
    
}


