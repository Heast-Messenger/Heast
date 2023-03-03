using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Permissionengine;

namespace Chat.Model;

[Table("permissionclients")]
public class PermissionClient
{
    [Key]
    public int PermissionClientId { get; set; }
    public string Name { get; set; }

    public PermissionClient(string name, int permissionClientId)
    {
        Name = name;
        PermissionClientId = permissionClientId;
    }

    public bool HasPermission(int pid)
    {
        return PermissionsEngine.HasPermission(PermissionClientId, pid);
    }
}


