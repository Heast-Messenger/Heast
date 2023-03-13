using System.Collections;
using Chat.Modules;
using Chat.Permissionengine.Permissions;
using Chat.Permissionengine.Permissions.Identifiers;
using Chat.Utility;
using Newtonsoft.Json;

namespace Chat.Permissionengine;

public class PermissionsEngine
{
    private static HashSet<Permission> _permissions = new();
    public static IReadOnlySet<Permission> Permissions => _permissions;
    private static Dictionary<int, BitArray> PriorityPermissions { get; } = new();

    public static void Init()
    {
        var text = File.ReadAllText("resources/permissions.json");
        _permissions = JsonConvert.DeserializeObject<HashSet<Permission>>(text)!;
    }
    
    
    private static BitArray DeterminePriorityPermissions(int id)
    {
        return BitArrayUtil.CascadeBitArrayList(Database.GetPermissionListOfClient(id));
    }
    
    /*Permission Querying*/
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

    /*Creation and User connection*/
    public static void UserConnect(int id)
    {
        if (Database.GetUserById(id) == null)
        {
            PriorityPermissions.Add(id, DeterminePriorityPermissions(Database.CreateUser()));
        }else PriorityPermissions.Add(id, DeterminePriorityPermissions(id));
        //TODO: send all roles per user to client on connect
    }

    public static int CreateChannel(string name)
    {
        return Database.CreateChannel(name);
    }

    public static int CreateRole(string name, int hierarchy, BitArray permissions)
    {
        return Database.CreateRole(name, hierarchy, permissions);
    }

    public static int CreateUser()
    {
        return Database.CreateUser();
    }
}