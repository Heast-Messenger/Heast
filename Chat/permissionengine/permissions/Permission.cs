namespace ChatServer.permissionengine.permissions;

public class Permission
{
    public string Description { get; }
    public string Name { get; }
    public int Id { get; }
    public PermissionTarget Target { get;}
    
    public Permission(string name, string description, int id, PermissionTarget target)
    {
        Name = name;
        Description = description;
        Id = id;
        Target = target;
    }
}