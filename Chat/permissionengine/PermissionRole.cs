using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatServer.permissionengine;

[Table("permissionroles")]
public class PermissionRole
{
    [Key]
    public int PermissionRoleId { get; set; }
    public string Name { get; set; }
    public int Hierarchy { get; set; }
    
    public BitArray Permissions { get; set; }

    public PermissionRole(string name, int permissionRoleId, int hierarchy, BitArray permissions)
    {
        Name = name;
        PermissionRoleId = permissionRoleId;
        Hierarchy = hierarchy;
        Permissions = permissions;
    }

}