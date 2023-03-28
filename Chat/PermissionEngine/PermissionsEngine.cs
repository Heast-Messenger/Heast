using System.Collections;
using Chat.Modules;
using Chat.Permissionengine.Permissions;
using Chat.Permissionengine.Permissions.Identifiers;
using Chat.Utility;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Chat.Permissionengine;

public class PermissionsEngine
{
    private static HashSet<Permission> _permissions = new();
    public static IReadOnlySet<Permission> Permissions => _permissions;
    private static Dictionary<int, BitArray> PriorityPermissions { get; } = new();

    public const int RolePermissionMaxSize = 255;
    public const int ChannelPermissionMaxSize = 512;

    public static void Init()
    {
        var text = File.ReadAllText("resources/permissions.json");
        _permissions = JsonConvert.DeserializeObject<HashSet<Permission>>(text)!;
    }
    
    
    private static BitArray DeterminePriorityPermissions(int id)
    {
        return BitArrayUtil.CascadeBitArrayList(Database.GetPermissionListOfClient(id));
    }

    private static bool UpdatePriorityPermissions(int id)
    {
       /* 
        Console.WriteLine(JsonSerializer.Serialize(Database.Ctx.Roles.First(x => x.PermissionRoleId == 1).Permissions));
        Console.WriteLine("----------");
        Console.WriteLine(JsonSerializer.Serialize(Database.GetPermissionListOfClient(id)));
        Console.WriteLine("---------");
        Console.WriteLine(JsonSerializer.Serialize(DeterminePriorityPermissions(id)));
        */
        if (PriorityPermissions.ContainsKey(id))
        {
            PriorityPermissions[id] = DeterminePriorityPermissions(id);
            return true;
        }

        PriorityPermissions.Add(id, DeterminePriorityPermissions(id));
        return true;
    }

    /*Permission Querying*/
    public static bool HasPermission(int uid, int pid)
    {
        return PriorityPermissions[uid][pid];
    }

    public static bool SetChannelPermission(int cid, int rid, int pid, bool value)
    {
        return Database.SetChannelPermission(cid, rid, pid, value);
        //TODO maybe redetermine channelperms?
    }

    public static bool SetPermission(int rid, int pid, bool value)
    {
        var sol = Database.SetPermission(rid, pid, value);

        if (!sol) return sol;
        foreach (var client in Database.GetClientsWithRole(rid))
        {
            UpdatePriorityPermissions(client);
        }

        return sol;
    }
    

    public static bool SetRole(int uid, int rid, bool value)
    {
        var sol = Database.SetRole(uid, rid, value);
        
        if (!sol) return sol;
        foreach (var client in Database.GetClientsWithRole(uid))
        {
            UpdatePriorityPermissions(client);
        }

        return sol;
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
        PriorityPermissions.Add(id, Database.GetUserById(id) == null
                ? DeterminePriorityPermissions(Database.CreateUser())
                : DeterminePriorityPermissions(id));
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
        int id = Database.CreateUser();
        UpdatePriorityPermissions(id);
        return id;
    }

    public static int CreateUser(int role)
    {
        int id = Database.CreateUser(role);
        UpdatePriorityPermissions(id);
        return id;
    }
}