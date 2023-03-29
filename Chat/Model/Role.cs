using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

[Table("roles")]
public class Role
{
    [Key]
    public int RoleId { get; set; }
    public string Name { get; set; }
    public int Hierarchy { get; set; }
    public BitArray Permissions { get; set; }

    public Role(string name, int hierarchy, BitArray permissions)
    {
        Name = name;
        Hierarchy = hierarchy;
        Permissions = permissions;
    }
}