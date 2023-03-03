using System.Collections;
using System.Globalization;
using ChatServer.network;
using ChatServer.permissionengine.permissions;
using ChatServer.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ChatServer.permissionengine;

public class PermissionsEngine
{
    
    //TODO Permissionchannel and channel permissions (ChannelVisibleToClient(int cid, int uid))
    
    private HashSet<Permission> _permissions = new();
    public IReadOnlySet<Permission> Permissions => _permissions;
    private static Dictionary<int, BitArray> PriorityPermissions { get; } = new Dictionary<int, BitArray>();

    //TODO: Read permissions from JSON this this this
    //TODO: send all roles per user to client on connect
    public static void Init()
    {
        string text = File.ReadAllText("resources/permissions.json");
        var permissions = JsonConvert.DeserializeObject<HashSet<Permission>>(text);
    }

    public static void UserConnect(int id)
    {
        //TODO EXECUTE ON USER CONNECT
        PriorityPermissions.Add(id, BitArrayUtil.CascadeBitArrayList(Database.GetPermissionListOfClient(id)));
    }

    public static bool HasPermission(int uid, int pid)
    {
        return PriorityPermissions[uid][pid];
    }
}