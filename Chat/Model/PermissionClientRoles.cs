using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

[Table("permissionclientroles")]
public class PermissionClientRoles
{
    [Key]
    public int PermissionClientId { get; set; }
    public PermissionClient PermissionClient { get; set; }
    
    [Key]
    public int PermissionRoleId { get; set; }
    public PermissionRole PermissionRole { get; set; }

    public PermissionClientRoles(int permissionClientId, int permissionRoleId)
    {
        PermissionClientId = permissionClientId;
        PermissionRoleId = permissionRoleId;
    }
}