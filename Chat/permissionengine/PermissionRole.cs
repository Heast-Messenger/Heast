using System.Collections;

namespace ChatServer.permissionengine;

public class PermissionRole
{
    public string Name { get; }
    public int Id { get; }
    public int Hierarchy { get; }
    
    //TODO: init permission with correct size, possibly with the use of the PermissionHandler getGlobalPermissionAmount() method
    public BitArray Permissions { get; }

    public PermissionRole(string name, int id, int hierarchy, BitArray permissions)
    {
        Name = name;
        Id = id;
        Hierarchy = hierarchy;
        Permissions = permissions;
    }

    public static PermissionRole GetRoleFromDb(int id)
    {
        //TODO: get role from database
        return null;
    }
    
}