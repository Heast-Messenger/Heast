using System.Collections;
using ChatServer.network;
using ChatServer.permissionengine.permissions;
using ChatServer.permissionengine.permissions.identifiers;
using ChatServer.util;
using Newtonsoft.Json;

namespace ChatServer.permissionengine;

public class PermissionsEngine
{
    
    //TODO Permissionchannel and channel permissions (ChannelVisibleToClient(int cid, int uid))
    
    private HashSet<Permission> _permissions = new();
    public IReadOnlySet<Permission> Permissions => _permissions;
    private static Dictionary<int, BitArray> PriorityPermissions { get; } = new Dictionary<int, BitArray>();

    public static void Init()
    {
        string text = File.ReadAllText("resources/permissions.json");
        var permissions = JsonConvert.DeserializeObject<HashSet<Permission>>(text);
    }
    
    public static void UserConnect(int id)
    {
        PriorityPermissions.Add(id, DeterminePriorityPermissions(id));
        //TODO: send all roles per user to client on connect
    }

    private static BitArray DeterminePriorityPermissions(int id)
    {
        return BitArrayUtil.CascadeBitArrayList(Database.GetPermissionListOfClient(id));
    }

    public static bool HasPermission(int uid, int pid)
    {
        return PriorityPermissions[uid][pid];
    }

    public static bool HasChannelPermissionForRole(int cid, int pid, int rid)
    {
        return Database.GetRolePermissionsOfChannel(cid, rid)[pid];
    }

    public static bool ChannelVisibleToClient(int cid, int uid)
    {
        var b = BitArrayUtil.CascadeBitArrayList(Database.GetUserRolePermissionListOfChannel(uid, cid));
        return b[(int) ChannelPermissionIdentifiers.See];
    }
}