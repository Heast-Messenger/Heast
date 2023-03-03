using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatServer.permissionengine;

[Table("permissionchannelpermissions")]
public class PermissionChannelPermissions
{
    [Key]
    public int PermissionChannelId { get; set; }
    public PermissionChannel PermissionChannel { get; set; }
    
    [Key]
    public int PermissionRoleId { get; set; }
    public PermissionRole PermissionRole { get; set; }
    
    public BitArray Permissions { get; set; }
    
}