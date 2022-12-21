using System.Globalization;
using ChatServer.permissionengine.permissions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ChatServer.permissionengine;

public class PermissionsEngine
{
    private HashSet<Permission> _permissions = new();
    public IReadOnlySet<Permission> Permissions => _permissions;
    
    //TODO: Generate Priority role, not on server start, but on user connect
    //TODO: Read permissions from JSON
    //TODO: send all roles per user to client on connect
    public static void Init()
    {
        //Console.WriteLine(JsonConvert.SerializeObject(new SimpleTargetPermission("test", "test", 2, PermissionTarget.ALL)));
        
        string text = File.ReadAllText("resources/permissions.json");
        var permissions = JsonConvert.DeserializeObject<HashSet<Permission>>(text);
        
        Console.WriteLine(JsonSerializer.Serialize(permissions));
    }
}