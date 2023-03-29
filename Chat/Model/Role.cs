using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Model;

public class Role
{
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