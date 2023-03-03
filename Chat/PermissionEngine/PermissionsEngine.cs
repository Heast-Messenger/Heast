using System.Collections;
using Chat.Modules;
using Chat.Permissionengine.Permissions;
using Chat.Permissionengine.Permissions.Identifiers;
using Chat.Utility;
using Newtonsoft.Json;

namespace Chat.Permissionengine;

public class PermissionsEngine
{
    private HashSet<Permission> _permissions = new();
    public IReadOnlySet<Permission> Permissions => _permissions;
    private static Dictionary<int, BitArray> PriorityPermissions { get; } = new();

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