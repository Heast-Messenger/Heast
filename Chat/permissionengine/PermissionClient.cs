namespace ChatServer.permissionengine;

public class PermissionClient
{
    public string Name { get; }
    public int Id { get; }

    private List<PermissionRole> _roles = new();
    public IReadOnlyList<PermissionRole> Roles => _roles;

    public PermissionClient(string name, int id, List<PermissionRole> roles)
    {
        Name = name;
        Id = id;
        _roles = roles;
    }
    
    public static PermissionClient GetClientFromDB(int id)
    {
        //TODO get client from the database with the DB system
        return null;
    }
}