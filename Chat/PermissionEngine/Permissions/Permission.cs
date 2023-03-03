namespace Chat.Permissionengine.Permissions;

public class Permission
{
    public string Description { get; }
    public string Name { get; }
    public int PermissionId { get; }
    public PermissionTarget Target { get;}
    
    public Permission(string name, string description, int id, PermissionTarget target)
    {
        Name = name;
        Description = description;
        PermissionId = id;
        Target = target;
    }
}