using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatServer.permissionengine;

[Table("permissionclientroles")]
public class PermissionClientRoles
{
    [Key]
    public int PermissionClientId { get; set; }
    public PermissionClient PermissionClient { get; set; }
    
    [Key]
    public int PermissionRoleId { get; set; }
    public PermissionRole PermissionRole { get; set; }
}